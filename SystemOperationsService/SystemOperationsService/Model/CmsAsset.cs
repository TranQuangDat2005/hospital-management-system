using System;

namespace SystemOperationsService.Model
{
    public enum AssetType
    {
        Banner = 0,
        News = 1,
        Announcement = 2
    }

    public class CmsAsset
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public AssetType Type { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public bool IsPublished { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
