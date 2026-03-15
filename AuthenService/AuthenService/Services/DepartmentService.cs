using User_Authentication_Service.DTOs;
using User_Authentication_Service.Interfaces;
using User_Authentication_Service.Model;

namespace User_Authentication_Service.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _deptRepo;

        public DepartmentService(IDepartmentRepository deptRepo)
        {
            _deptRepo = deptRepo;
        }

        public async Task<IEnumerable<DepartmentResponseDto>> GetAllAsync(bool includeInactive = true)
        {
            var depts = await _deptRepo.GetAllAsync(includeInactive);
            return depts.Select(MapToDto);
        }

        public async Task<DepartmentResponseDto?> GetByIdAsync(int id)
        {
            var dept = await _deptRepo.GetByIdAsync(id);
            return dept == null ? null : MapToDto(dept);
        }

        public async Task<(bool Success, string Message, DepartmentResponseDto? Data)> CreateAsync(CreateDepartmentDto dto)
        {
            var existing = await _deptRepo.GetByNameAsync(dto.Name);
            if (existing != null)
                return (false, $"Phòng ban '{dto.Name}' đã tồn tại.", null);

            var dept = new Departments
            {
                Name        = dto.Name.Trim(),
                Location    = dto.Location.Trim(),
                Description = dto.Description.Trim(),
                Status      = "Inactive",
                IsDeleted   = false,
                CreatedAt   = DateTime.UtcNow
            };

            await _deptRepo.AddAsync(dept);
            await _deptRepo.SaveChangesAsync();

            return (true, "Tạo phòng ban thành công. Trạng thái mặc định: Inactive.", MapToDto(dept));
        }

        public async Task<(bool Success, string Message, DepartmentResponseDto? Data)> UpdateAsync(int id, UpdateDepartmentDto dto)
        {
            var dept = await _deptRepo.GetByIdAsync(id);
            if (dept == null)
                return (false, $"Không tìm thấy phòng ban ID={id}.", null);
            if (!string.IsNullOrWhiteSpace(dto.Name) && dto.Name != dept.Name)
            {
                var nameExists = await _deptRepo.GetByNameAsync(dto.Name);
                if (nameExists != null)
                    return (false, $"Tên phòng ban '{dto.Name}' đã được sử dụng.", null);
                dept.Name = dto.Name.Trim();
            }

            if (!string.IsNullOrWhiteSpace(dto.Location))    dept.Location    = dto.Location.Trim();
            if (!string.IsNullOrWhiteSpace(dto.Description)) dept.Description = dto.Description.Trim();
            if (!string.IsNullOrWhiteSpace(dto.Status))     dept.Status     = dto.Status;

            _deptRepo.Update(dept);
            await _deptRepo.SaveChangesAsync();

            return (true, "Cập nhật phòng ban thành công.", MapToDto(dept));
        }

        public async Task<(bool Success, string Message)> DeleteAsync(int id)
        {
            var dept = await _deptRepo.GetByIdAsync(id);
            if (dept == null)
                return (false, $"Không tìm thấy phòng ban ID={id}.");
            var hasActiveUsers = await _deptRepo.HasActiveUsersAsync(id);
            if (hasActiveUsers)
                return (false, "Không thể xóa phòng ban đang có nhân viên hoạt động. Vui lòng chuyển hoặc ngừng hoạt động các nhân viên trước.");
            await _deptRepo.SoftDeleteAsync(id);
            await _deptRepo.SaveChangesAsync();

            return (true, "Phòng ban đã được xóa khỏi hệ thống (dữ liệu lịch sử vẫn được bảo toàn).");
        }

        private static DepartmentResponseDto MapToDto(Departments d) => new()
        {
            DepartmentID    = d.DepartmentID,
            Name            = d.Name,
            Location        = d.Location,
            Description     = d.Description,
            Status          = d.Status,
            CreatedAt       = d.CreatedAt,
            ActiveUserCount = d.Users.Count(u => u.Status == "Active")
        };
    }
}
