using Microsoft.EntityFrameworkCore;
using Mottu.Domain.Entities;
using System.Reflection;

namespace Mottu.Infrastructure.Persistence.PostgreSQL;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Motorcycle> Motorcycles { get; set; } = null!;
    public DbSet<DeliveryDriver> DeliveryDrivers { get; set; } = null!;
    public DbSet<Rental> Rentals { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply all entity configurations from current assembly
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}

