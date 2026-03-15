using Microsoft.AspNetCore.Mvc;
using Patient_Scheduling_Service.DTOs;
using Patient_Scheduling_Service.Interfaces;
using System.Security.Claims;

namespace Patient_Scheduling_Service.Controllers
{
    /* 
     * 1. Vai trò và Người dùng chính
     * - Vai trò: Quản lý quy trình tiếp nhận bệnh nhân ban đầu (Patient intake), bao gồm việc xác thực danh tính và điều phối lịch hẹn để tối ưu hóa hàng đợi tại phòng khám.
     * - Guest (Khách): Người dùng chưa đăng nhập, thực hiện đăng ký tài khoản.
     * - Patient (Bệnh nhân): Người dùng đã có tài khoản, thực hiện đặt lịch và theo dõi lịch cá nhân.
     * - Receptionist (Lễ tân): Nhân viên y tế thực hiện tra cứu, định danh hoặc đăng ký trực tiếp cho bệnh nhân tại quầy.
     */
    [ApiController]
    [Route("api/[controller]")]
    public class PatientsController : ControllerBase
    {
        private readonly IPatientService _patientService;

        public PatientsController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        /*
         * 2. Đăng ký tài khoản và hồ sơ (UC-PA-34, UC-RC-07): Cho phép tạo mới thông tin hành chính của bệnh nhân bao gồm: Họ tên, Ngày sinh, Giới tính, Số điện thoại, Địa chỉ và số CCCD (12 chữ số).
         * 3. Quy trình vận hành (Flow) - Kiểm tra điều kiện: Hệ thống xác thực số CCCD là duy nhất theo quy tắc BR1.
         * 5. Ràng buộc nghiệp vụ (Business Rules):
         *    - BR1: Mỗi bệnh nhân chỉ có một ID duy nhất dựa trên 12 số CCCD; hệ thống chặn tạo hồ sơ trùng lặp.
         *    - BR7: Do không có phần cứng quét chuyên dụng, mọi thông tin CCCD (12 số) phải được kiểm tra định dạng qua Regex trước khi lưu.
         */
        [HttpPost("register")]
        [Microsoft.AspNetCore.Authorization.AllowAnonymous] // Guest hoặc Lễ tân (Receptionist) đều có thể tạo hồ sơ mới
        public async Task<IActionResult> Register([FromBody] PatientRegistrationDto dto)
        {
            try
            {
                // Trích xuất UserID từ Token nếu người dùng đã đăng nhập (ví dụ: Guest vừa tạo tài khoản xong, có token nhưng chưa có profile)
                if (User.Identity?.IsAuthenticated == true)
                {
                    var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    if (int.TryParse(userIdClaim, out int loggedInUserId))
                    {
                        dto.UserID = loggedInUserId;
                    }
                }

                var result = await _patientService.RegisterPatientAsync(dto);
                return Ok(new { message = "Hồ sơ bệnh nhân đã được tạo thành công.", patientId = result.PatientID });
            }
            catch (ArgumentException ex)
            {
                // Bắt lỗi Validation Logic từ Service (như CCCD trùng)
                return BadRequest(new { message = ex.Message });
            }
        }

        /*
         * 2. Định danh bệnh nhân (UC-RC-06): Tra cứu hồ sơ đã tồn tại thông qua quét mã QR trên CCCD hoặc nhập thủ công số điện thoại/CCCD để bắt đầu quy trình khám.
         * 3. Quy trình vận hành (Flow) - Xác nhận tại quầy: Lễ tân thực hiện tìm kiếm hồ sơ. Nếu không có, hệ thống chuyển sang quy trình tạo hồ sơ mới trước khi phân luồng vào phòng khám.
         * 4. Trạng thái và Chuyển đổi trạng thái
         *    A. Trạng thái Hồ sơ bệnh nhân (Patient Profile):
         *       - None: Trạng thái bắt đầu.
         *       - Searching: Khi người dùng nhập CCCD/Số điện thoại để tra cứu.
         *       - Creating: Chuyển sang khi không tìm thấy hồ sơ (ProfileNotFound).
         *       - Registered/Identified: Trạng thái cuối khi hồ sơ đã được tạo mới hoặc tìm thấy thành công.
         */
        [HttpGet("identify")]
        [Microsoft.AspNetCore.Authorization.Authorize(Roles = "Receptionist,Admin")] // Lễ tân hoặc Admin định danh tại quầy
        public async Task<IActionResult> Identify([FromQuery] string? cccd, [FromQuery] string? phone)
        {
            try
            {
                var patient = await _patientService.IdentifyPatientAsync(cccd, phone);
                if (patient == null)
                {
                    // Logic: Query DB. Nếu ProfileNotFound -> Chuyển State -> Creating (điều hướng sang flow tạo mới)
                    // (Điều hướng này thường do Frontend thực hiện dựa vào mã 404 trả về)
                    return NotFound(new { message = "Không tìm thấy hồ sơ. Đang chuyển hướng sang quy trình tạo hồ sơ mới (Creating State)." });
                }

                return Ok(new { message = "Định danh thành công. (State -> Registered/Identified)", patient });
            }
            catch(ArgumentException ex)
            {
               return BadRequest(new { message = ex.Message });
            }
        }
    }
}
