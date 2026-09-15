using CareLink360.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CareLink360.Context
{
    public class ApplicationDbContext
        : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Client> Clients { get; set; }

        public DbSet<Caregiver> Caregivers { get; set; }

        public DbSet<NinVerification> NinVerifications { get; set; }

        public DbSet<JobType> JobTypes { get; set; }

        public DbSet<MedicalRecord> MedicalRecords { get; set; }

        public DbSet<JobRequest> JobRequests { get; set; }

        public DbSet<Contract> Contracts { get; set; }

        public DbSet<Payment> Payments { get; set; }

        public DbSet<Rating> Ratings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<JobType>()
                .Property(x => x.BaseRate)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Payment>()
                .Property(x => x.Amount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Caregiver>()
                .Property(x => x.AverageRating)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Caregiver>()
                .HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Client>()
                .HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Contract>()
               .Property(c => c.Status)
               .HasConversion<string>();

            modelBuilder.Entity<Contract>()
                .Property(c => c.TotalAmount)
                .HasPrecision(18, 2);


        }
    }
}