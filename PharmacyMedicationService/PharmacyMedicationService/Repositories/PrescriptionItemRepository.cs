using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PharmacyMedicationService.Data;
using PharmacyMedicationService.Interfaces;
using PharmacyMedicationService.Model;

namespace PharmacyMedicationService.Repositories
{
    public class PrescriptionItemRepository : IPrescriptionItemRepository
    {
        private readonly PharmacyMedicationDbContext _context;

        public PrescriptionItemRepository(PharmacyMedicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PrescriptionItem>> GetByPrescriptionIdAsync(Guid prescriptionId)
        {
            return await _context.PrescriptionItems
                .Include(p => p.Medication)
                .Where(p => p.PrescriptionId == prescriptionId)
                .ToListAsync();
        }

        public async Task<IEnumerable<PrescriptionItem>> GetByIdsAsync(IEnumerable<Guid> itemIds)
        {
            return await _context.PrescriptionItems
                .Where(p => itemIds.Contains(p.Id))
                .ToListAsync();
        }

        public async Task UpdateItemsAsync(IEnumerable<PrescriptionItem> items)
        {
            _context.PrescriptionItems.UpdateRange(items);
            await _context.SaveChangesAsync();
        }

        public async Task<Medication?> GetMedicationByIdAsync(Guid medicationId)
        {
            return await _context.Medications.FindAsync(medicationId);
        }
    }
}
