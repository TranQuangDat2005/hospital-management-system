using System;
using SystemOperationsService.Model;

namespace SystemOperationsService.DTOs
{
    public class CreateCmsAssetDto
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public AssetType Type { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public bool IsPublished { get; set; }
    }

    public class UpdateCmsAssetDto
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public AssetType Type { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public bool IsPublished { get; set; }
    }

    public class CmsAssetResponseDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Type { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public bool IsPublished { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
