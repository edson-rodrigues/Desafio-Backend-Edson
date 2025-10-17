using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mottu.Domain.Entities;
using Mottu.Domain.ValueObjects;

namespace Mottu.Infrastructure.Persistence.PostgreSQL.Configurations;

public class MotorcycleConfiguration : IEntityTypeConfiguration<Motorcycle>
{
    public void Configure(EntityTypeBuilder<Motorcycle> builder)
    {
        builder.ToTable("motorcycles");

        builder.HasKey(m => m.Id);

        builder.Property(m => m.Id)
            .HasColumnName("id")
            .IsRequired();

        builder.Property(m => m.Identifier)
            .HasColumnName("identifier")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(m => m.Year)
            .HasColumnName("year")
            .IsRequired();

        builder.Property(m => m.Model)
            .HasColumnName("model")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(m => m.LicensePlate)
            .HasColumnName("license_plate")
            .HasMaxLength(20)
            .IsRequired()
            .HasConversion(
                v => v.Value,
                v => LicensePlate.Create(v));

        builder.Property(m => m.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(m => m.UpdatedAt)
            .HasColumnName("updated_at");

        // Indexes
        builder.HasIndex(m => m.LicensePlate)
            .IsUnique()
            .HasDatabaseName("ix_motorcycles_license_plate");

        builder.HasIndex(m => m.Identifier)
            .HasDatabaseName("ix_motorcycles_identifier");
    }
}

