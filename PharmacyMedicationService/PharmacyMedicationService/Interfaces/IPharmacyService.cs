using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PharmacyMedicationService.DTOs;
using PharmacyMedicationService.Model;

namespace PharmacyMedicationService.Interfaces
{
    public interface IPharmacyService
    {
        Task<IEnumerable<PrescriptionResponseDto>> GetPrescriptionQueueAsync();
        Task<PrescriptionDetailDto?> GetPrescriptionDetailAsync(Guid prescriptionId);
        Task<IEnumerable<PrescriptionItemResponseDto>> GetPrescriptionItemsAsync(Guid prescriptionId);
        Task<bool> UpdateItemStatusAsync(UpdateItemStatusDto request);
    }
}
