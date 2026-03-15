using ClinicalExaminationService.DTOs;
using ClinicalExaminationService.Model;

namespace ClinicalExaminationService.Interfaces
{
    public interface IExaminationService
    {
        Task<VisitRecordResponseDto> CreateVisitAsync(CreateVisitRecordDto dto);
        Task<VisitRecordResponseDto?> GetVisitAsync(Guid id);
        Task<IEnumerable<VisitRecordResponseDto>> GetAllVisitsAsync(VisitStatus? status);
        Task<VisitRecordResponseDto> UpdateStatusAsync(Guid id, VisitStatus newStatus);
        Task<VisitRecordResponseDto> TogglePriorityAsync(Guid id);
        Task<VisitRecordResponseDto> RecordVitalSignsAsync(RecordVitalSignsDto dto);
        Task<VisitRecordResponseDto> DiagnoseAsync(DiagnoseRequestDto dto);
    }
}
