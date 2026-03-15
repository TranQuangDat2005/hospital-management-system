using FinancialBillingService.Model;
using Microsoft.EntityFrameworkCore;

namespace FinancialBillingService.Data
{
    public class FinancialBillingDbContext : DbContext
    {
        public FinancialBillingDbContext(DbContextOptions<FinancialBillingDbContext> options) : base(options)
        {
        }

        public DbSet<HealthInsurance> HealthInsurance { get; set; }
        public DbSet<Invoice> Invoice { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet <ServiceOrder> ServiceOrders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(FinancialBillingDbContext).Assembly);
        }
    }
}
