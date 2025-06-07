using CarReservations.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace CarReservations.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Car> Cars => Set<Car>();
        public DbSet<Reservation> Reservations => Set<Reservation>();
        public DbSet<Customer> Customers => Set<Customer>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Car>()
                .HasMany(c => c.Reservations)
                .WithOne(r => r.Car)
                .HasForeignKey(r => r.CarId);
            modelBuilder.Entity<Customer>()
                .HasMany(c => c.Reservations)
                .WithOne(r => r.Customer)
                .HasForeignKey(r => r.CustomerId);
        }
    }
}
