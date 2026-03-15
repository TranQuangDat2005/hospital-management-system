using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PharmacyMedicationService.Model;

namespace PharmacyMedicationService.Interfaces
{
    public interface IPrescriptionItemRepository
    {
        Task<IEnumerable<PrescriptionItem>> GetByPrescriptionIdAsync(Guid prescriptionId);
        Task<IEnumerable<PrescriptionItem>> GetByIdsAsync(IEnumerable<Guid> itemIds);
        Task UpdateItemsAsync(IEnumerable<PrescriptionItem> items);
        Task<Medication?> GetMedicationByIdAsync(Guid medicationId);
    }
}
