using Microsoft.EntityFrameworkCore;
using SystemOperationsService.Model;

namespace SystemOperationsService.Data
{
    public class OpsDbContext : DbContext
    {
        public OpsDbContext(DbContextOptions<OpsDbContext> options) : base(options) { }

        public DbSet<BugReport> BugReports { get; set; }
        public DbSet<CmsAsset> CmsAssets { get; set; }
    }
}
