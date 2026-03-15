using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SystemOperationsService.DTOs;
using SystemOperationsService.Model;
using SystemOperationsService.Repositories;

namespace SystemOperationsService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CmsController : ControllerBase
    {
        private readonly ICmsAssetRepository _repo;

        public CmsController(ICmsAssetRepository repo)
        {
            _repo = repo;
        }

        
        [HttpGet("active")]
        [AllowAnonymous]
        public async Task<IActionResult> GetActiveAssets([FromQuery] AssetType? type)
        {
            var assets = await _repo.GetAllAsync(isPublished: true, type: type);
            var response = assets.Select(a => new CmsAssetResponseDto
            {
                Id = a.Id,
                Title = a.Title,
                Description = a.Description,
                Type = a.Type.ToString(),
                ImageUrl = a.ImageUrl,
                IsPublished = a.IsPublished,
                CreatedAt = a.CreatedAt
            });
            return Ok(response);
        }

        
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllAssets([FromQuery] bool? isPublished, [FromQuery] AssetType? type)
        {
            var assets = await _repo.GetAllAsync(isPublished, type);
            var response = assets.Select(a => new CmsAssetResponseDto
            {
                Id = a.Id,
                Title = a.Title,
                Description = a.Description,
                Type = a.Type.ToString(),
                ImageUrl = a.ImageUrl,
                IsPublished = a.IsPublished,
                CreatedAt = a.CreatedAt
            });
            return Ok(response);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAssetById(Guid id)
        {
            var asset = await _repo.GetByIdAsync(id);
            if (asset == null) return NotFound();

            return Ok(new CmsAssetResponseDto
            {
                Id = asset.Id,
                Title = asset.Title,
                Description = asset.Description,
                Type = asset.Type.ToString(),
                ImageUrl = asset.ImageUrl,
                IsPublished = asset.IsPublished,
                CreatedAt = asset.CreatedAt
            });
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateAsset([FromBody] CreateCmsAssetDto dto)
        {
            var asset = new CmsAsset
            {
                Id = Guid.NewGuid(),
                Title = dto.Title,
                Description = dto.Description,
                Type = dto.Type,
                ImageUrl = dto.ImageUrl,
                IsPublished = dto.IsPublished,
                CreatedAt = DateTime.UtcNow
            };

            await _repo.CreateAsync(asset);
            return CreatedAtAction(nameof(GetAssetById), new { id = asset.Id }, asset);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateAsset(Guid id, [FromBody] UpdateCmsAssetDto dto)
        {
            var asset = await _repo.GetByIdAsync(id);
            if (asset == null) return NotFound();

            asset.Title = dto.Title;
            asset.Description = dto.Description;
            asset.Type = dto.Type;
            asset.ImageUrl = dto.ImageUrl;
            asset.IsPublished = dto.IsPublished;

            await _repo.UpdateAsync(asset);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAsset(Guid id)
        {
            var asset = await _repo.GetByIdAsync(id);
            if (asset == null) return NotFound();

            await _repo.DeleteAsync(id);
            return NoContent();
        }
    }
}

