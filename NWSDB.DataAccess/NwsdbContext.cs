
using System;
using Microsoft.EntityFrameworkCore;
using NWSDB.DataAccess.Entities;

namespace NWSDB.DataAccess
{
    public class NwsdbContext : DbContext
    {
        public NwsdbContext(DbContextOptions<NwsdbContext> options) : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; } = null!;
        public DbSet<Bill> Bills { get; set; } = null!;
        public DbSet<Payment> Payments { get; set; } = null!;
        public DbSet<UsageRecord> UsageRecords { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.AccountNumber).IsUnique();
                entity.Property(e => e.AccountNumber).IsRequired().HasMaxLength(50);
                entity.Property(e => e.FullName).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Status).HasMaxLength(50);
            });

            modelBuilder.Entity<Bill>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.BillNumber).IsUnique(false);
                entity.Property(e => e.BillNumber).IsRequired().HasMaxLength(100);

                // Configure monetary fields with appropriate precision
                entity.Property(e => e.UsageCharge)
                    .HasColumnType("decimal(18,2)");

                entity.Property(e => e.ServiceCharge)
                    .HasColumnType("decimal(18,2)");

                entity.Property(e => e.OtherCharges)
                    .HasColumnType("decimal(18,2)");

                entity.Property(e => e.TotalAmount)
                    .HasColumnType("decimal(18,2)");

                entity.HasOne(e => e.Customer)
                    .WithMany(c => c.Bills)
                    .HasForeignKey(e => e.CustomerId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.PaymentReference)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Amount)
                    .HasColumnType("decimal(18,2)");

                // Prevent SQL Server multiple cascade path error.
                entity.HasOne(e => e.Customer)
                    .WithMany(c => c.Payments)
                    .HasForeignKey(e => e.CustomerId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(e => e.Bill)
                    .WithMany()
                    .HasForeignKey(e => e.BillId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<UsageRecord>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.BillingMonth)
                    .HasMaxLength(50);

                entity.HasOne(e => e.Customer)
                    .WithMany(c => c.UsageRecords)
                    .HasForeignKey(e => e.CustomerId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.FullName)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.HasIndex(e => e.Email)
                    .IsUnique();

                entity.Property(e => e.AccountNumber)
                    .HasMaxLength(50);

                entity.Property(e => e.NIC)
                    .HasMaxLength(20);

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(30);

                entity.Property(e => e.Address)
                    .HasMaxLength(300);

                entity.Property(e => e.PasswordHash)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.Role)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.CreatedAt)
                    .IsRequired();
            });
        }
    }
     
}

