using Microsoft.AspNetCore.Mvc;
using Patient_Scheduling_Service.DTOs;
using Patient_Scheduling_Service.Interfaces;
using System.Security.Claims;

namespace Patient_Scheduling_Service.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentsController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentsController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        /*
         * 2. Đặt lịch hẹn trực tuyến (UC-PA-30): Bệnh nhân tự chọn chuyên khoa, bác sĩ, ngày khám và khung giờ còn trống, đồng thời mô tả sơ lược triệu chứng.
         * 3. Quy trình vận hành (Flow) - Đặt lịch:
         *    - Người dùng chọn khung giờ. Hệ thống kiểm tra khung giờ phải ở tương lai (BR22) và không bị trùng lặp với các lịch hẹn khác của cùng bệnh nhân (BR23).
         *    - Mô tả triệu chứng được lưu lại trong bảng appointments.
         * 4. B. Trạng thái Lịch hẹn (Appointment status):
         *    - Pending (Chờ xử lý): Trạng thái mặc định khi lịch hẹn được tạo thành công trên database.
         *    // Các trạng thái khác: Tài liệu không liệt kê rõ các trạng thái tiếp theo của lịch hẹn (ví dụ: Confirmed, Cancelled hay Arrived), ngoại trừ việc ghi nhận trạng thái khởi tạo là "Pending".
         *    // TODO: Cần làm rõ các trạng thái tiếp theo để sửa và cập nhật flow hoàn chỉnh sau.
         * 5. Ràng buộc nghiệp vụ (Business Rules):
         *    - BR15: Lịch hẹn không được phép trùng lặp khung giờ cho cùng một bệnh nhân (tương đương BR23).
         */
        [HttpPost("book")]
        [Microsoft.AspNetCore.Authorization.Authorize(Roles = "Patient,Receptionist,Admin")] // Bệnh nhân tự đặt, hoặc Lễ tân đặt hộ
        public async Task<IActionResult> Book([FromBody] AppointmentBookingDto dto)
        {
            try
            {
                // Lấy UserID (tương đương PatientID đối với Role Patient) từ JWT Sub claim
                var userIdClaim = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier);
                if (!int.TryParse(userIdClaim, out int loggedInUserId))
                {
                    return Unauthorized(new { message = "Không xác định được danh tính người dùng từ Token." });
                }

                // Ghi chú: Nếu Role là Lễ tân/Admin đang book hộ, DTO có thể cần truyền thêm PatientID, 
                // hoặc mặc định lấy loggedInUserId nếu là Role Patient.
                // Ở đây, giả định đơn giản patientId chính là loggedInUserId.
                int patientId = loggedInUserId; 
                
                var result = await _appointmentService.BookAppointmentAsync(patientId, dto);
                return Ok(new { message = "Lịch hẹn đã được lưu thành công.", appointmentId = result.AppointmentID, status = result.Status });
            }
            catch(ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /*
         * 2. Quản lý thông tin công khai: Xem danh sách bác sĩ (chuyên môn, kinh nghiệm) và danh mục dịch vụ cùng đơn giá để hỗ trợ quyết định đặt lịch.
         */
        [HttpGet("public-info")]
        [Microsoft.AspNetCore.Authorization.AllowAnonymous] // Ai cũng xem được, không cần token
        public IActionResult GetPublicInfo()
        {
            return Ok(new { message = "Đang phát triển chức năng xem thông tin công khai." });
        }
    }
}
