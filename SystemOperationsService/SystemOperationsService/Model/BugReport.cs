using System;

namespace SystemOperationsService.Model
{
    public enum BugStatus
    {
        Open = 0,
        InProgress = 1,
        Resolved = 2,
        Closed = 3
    }

    public enum BugPriority
    {
        Low = 0,
        Medium = 1,
        High = 2,
        Critical = 3
    }

    public class BugReport
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public BugStatus Status { get; set; } = BugStatus.Open;
        public BugPriority Priority { get; set; } = BugPriority.Medium;
        public string ReportedByUserId { get; set; } = string.Empty;
        public string? AssignedToUserId { get; set; }
        public string? ResolutionNotes { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
