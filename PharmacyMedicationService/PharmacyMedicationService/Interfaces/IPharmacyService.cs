using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PharmacyMedicationService.DTOs;
using PharmacyMedicationService.Model;

namespace PharmacyMedicationService.Interfaces
{
    public interface IPharmacyService
    {
        Task<IEnumerable<PrescriptionItemResponseDto>> GetPrescriptionItemsAsync(Guid prescriptionId);
        Task<bool> UpdateItemStatusAsync(UpdateItemStatusDto updateDto);
    }
}
