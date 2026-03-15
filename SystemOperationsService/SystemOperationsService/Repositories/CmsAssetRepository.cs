using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SystemOperationsService.Data;
using SystemOperationsService.Model;

namespace SystemOperationsService.Repositories
{
    public interface ICmsAssetRepository
    {
        Task<IEnumerable<CmsAsset>> GetAllAsync(bool? isPublished = null, AssetType? type = null);
        Task<CmsAsset?> GetByIdAsync(Guid id);
        Task<CmsAsset> CreateAsync(CmsAsset asset);
        Task UpdateAsync(CmsAsset asset);
        Task DeleteAsync(Guid id);
    }

    public class CmsAssetRepository : ICmsAssetRepository
    {
        private readonly OpsDbContext _context;

        public CmsAssetRepository(OpsDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CmsAsset>> GetAllAsync(bool? isPublished = null, AssetType? type = null)
        {
            var query = _context.CmsAssets.AsQueryable();

            if (isPublished.HasValue)
                query = query.Where(a => a.IsPublished == isPublished.Value);
            
            if (type.HasValue)
                query = query.Where(a => a.Type == type.Value);

            return await query.OrderByDescending(a => a.CreatedAt).ToListAsync();
        }

        public async Task<CmsAsset?> GetByIdAsync(Guid id)
        {
            return await _context.CmsAssets.FindAsync(id);
        }

        public async Task<CmsAsset> CreateAsync(CmsAsset asset)
        {
            _context.CmsAssets.Add(asset);
            await _context.SaveChangesAsync();
            return asset;
        }

        public async Task UpdateAsync(CmsAsset asset)
        {
            asset.UpdatedAt = DateTime.UtcNow;
            _context.CmsAssets.Update(asset);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var asset = await GetByIdAsync(id);
            if (asset != null)
            {
                _context.CmsAssets.Remove(asset);
                await _context.SaveChangesAsync();
            }
        }
    }
}
