using User_Authentication_Service.DTOs;

namespace User_Authentication_Service.Interfaces
{
    public interface IDepartmentService
    {
        /// <summary>Lấy tất cả phòng ban (trừ soft-deleted). includeInactive=false chỉ lấy Active (BR11).</summary>
        Task<IEnumerable<DepartmentResponseDto>> GetAllAsync(bool includeInactive = true);
        Task<DepartmentResponseDto?> GetByIdAsync(int id);
        Task<(bool Success, string Message, DepartmentResponseDto? Data)> CreateAsync(CreateDepartmentDto dto);
        Task<(bool Success, string Message, DepartmentResponseDto? Data)> UpdateAsync(int id, UpdateDepartmentDto dto);
        Task<(bool Success, string Message)> DeleteAsync(int id);
    }
}
