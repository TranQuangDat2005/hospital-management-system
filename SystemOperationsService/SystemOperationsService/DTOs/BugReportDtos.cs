using System;
using SystemOperationsService.Model;

namespace SystemOperationsService.DTOs
{
    public class CreateBugReportDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public BugPriority Priority { get; set; }
        public string ReportedByUserId { get; set; } = string.Empty;
    }

    public class UpdateBugStatusDto
    {
        public BugStatus Status { get; set; }
        public string? ResolutionNotes { get; set; }
        public string? AssignedToUserId { get; set; }
    }
}
