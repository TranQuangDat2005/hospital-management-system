using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PharmacyMedicationService.DTOs;
using PharmacyMedicationService.Interfaces;
using PharmacyMedicationService.Model;

namespace PharmacyMedicationService.Services
{
    public class PharmacyService : IPharmacyService
    {
        private readonly IPrescriptionItemRepository _repository;

        public PharmacyService(IPrescriptionItemRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<PrescriptionItemResponseDto>> GetPrescriptionItemsAsync(Guid prescriptionId)
        {
            var items = await _repository.GetByPrescriptionIdAsync(prescriptionId);
            return items.Select(item => new PrescriptionItemResponseDto
            {
                Id = item.Id,
                PrescriptionId = item.PrescriptionId,
                MedicationId = item.MedicationId,
                MedicationName = item.Medication?.Name ?? "Unknown",
                Quantity = item.Quantity,
                UnitPriceAtOrder = item.UnitPriceAtOrder,
                Status = item.Status.ToString(),
                UpdatedAt = item.UpdatedAt
            });
        }

        public async Task<bool> UpdateItemStatusAsync(UpdateItemStatusDto request)
        {
            if (request.ItemIds == null || !request.ItemIds.Any())
                throw new ArgumentException("ItemIds list cannot be empty.");

            var itemsToUpdate = await _repository.GetByIdsAsync(request.ItemIds);
            
            if (!itemsToUpdate.Any())
                return false;

            foreach (var item in itemsToUpdate)
            {
                if (request.NewStatus == ItemStatus.Paid)
                {
                    // Transition to Paid: Only from PendingPayment
                    if (item.Status != ItemStatus.PendingPayment)
                    {
                        throw new InvalidOperationException($"Cannot transition item {item.Id} to Paid from {item.Status}");
                    }

                    // BR-PH-04: Price Integrity - Lock the price at payment time
                    var medication = await _repository.GetMedicationByIdAsync(item.MedicationId);
                    if (medication != null)
                    {
                        item.UnitPriceAtOrder = medication.UnitPrice;
                    }
                }
                else if (request.NewStatus == ItemStatus.Dispensed)
                {
                    // BR-PH-01: Lock Dispense - System must throw error if transferring to Dispensed while PendingPayment
                    if (item.Status != ItemStatus.Paid)
                    {
                        throw new InvalidOperationException($"BR-PH-01 Violated: Cannot dispense item {item.Id} because it is not Paid. Current status: {item.Status}");
                    }
                    
                    // BR-PH-03: No Inventory Link - We only update status, no stock adjustment required.
                }

                // BR-PH-02: Item Independence - We update item strictly according to its own isolated logic
                item.Status = request.NewStatus;
                item.UpdatedAt = DateTime.UtcNow;
            }

            await _repository.UpdateItemsAsync(itemsToUpdate);
            return true;
        }
    }
}
