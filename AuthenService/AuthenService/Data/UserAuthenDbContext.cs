using Microsoft.EntityFrameworkCore;
using User_Authentication_Service.Model;
using User_Authentication_Service.ConfigModel;


namespace User_Authentication_Service.Data
{
    public class UserAuthenDbContext : DbContext
    {
        public UserAuthenDbContext(DbContextOptions<UserAuthenDbContext> options) : base(options)
        {
        }

        public DbSet<Users> Users { get; set; }
        public DbSet<Departments> Departments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserAuthenDbContext).Assembly);
        }

    }
}
