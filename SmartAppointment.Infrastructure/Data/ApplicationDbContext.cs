using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SmartAppointment.Domain.Entities;

namespace SmartAppointment.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser, IdentityRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

       // public DbSet<AppointmentModel> AppointmentsDetils { get; set; }
        public DbSet<Professional> Professionals { get; set; }
        public DbSet<ProfessionalAvailability> ProfessionalAvailabilities { get; set; }
        public DbSet<scheduleModel> scheduler { get; set; }

        public DbSet<AppointmentModel> Appointments { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AppointmentModel>()
             .HasOne(a => a.Scheduler)
             .WithMany(s => s.Appointments)
             .HasForeignKey(a => a.SchedulerId);

            modelBuilder.Entity<Professional>()
           .HasIndex(p => p.SLMC)
           .IsUnique();

            modelBuilder.Entity<scheduleModel>()
             .HasOne(s => s.Professional) // Schedule has one Professional
             .WithMany(p => p.scheduleModel) // Professional has many Schedules
             .HasForeignKey(s => s.ProfessionalId) // Foreign key in Schedule
             .OnDelete(DeleteBehavior.Cascade); // Optional: Define delete behavior



        }
    }



    
};
