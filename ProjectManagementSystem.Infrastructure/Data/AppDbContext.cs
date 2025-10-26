using Microsoft.EntityFrameworkCore;
namespace ProjectManagementSystem.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // --- Tables ---
        public DbSet<Tb_Project> Tb_Projects { get; set; }
        public DbSet<Tb_TaskItem> Tb_TaskItems { get; set; }
        public DbSet<Tb_User> Tb_Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}