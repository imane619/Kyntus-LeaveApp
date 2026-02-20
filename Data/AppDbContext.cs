using Microsoft.EntityFrameworkCore;
using LeaveService.Models;

namespace LeaveService.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<LeaveRequest> LeaveRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<LeaveRequest>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.EmployeeId).IsRequired();

                // CORRECTION ICI : On utilise la valeur de l'Enum, pas du texte
                entity.Property(e => e.Status)
                      .HasDefaultValue(LeaveStatus.Pending)
                      .IsRequired();

                entity.Property(e => e.StartDate).IsRequired();
                entity.Property(e => e.EndDate).IsRequired();
                
                entity.HasIndex(e => e.EmployeeId);
            });
        }
    }
}