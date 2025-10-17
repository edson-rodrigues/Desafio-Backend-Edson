using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mottu.Domain.Entities;
using Mottu.Domain.Enums;
using Mottu.Domain.ValueObjects;

namespace Mottu.Infrastructure.Persistence.PostgreSQL.Configurations;

public class DeliveryDriverConfiguration : IEntityTypeConfiguration<DeliveryDriver>
{
    public void Configure(EntityTypeBuilder<DeliveryDriver> builder)
    {
        builder.ToTable("delivery_drivers");

        builder.HasKey(d => d.Id);

        builder.Property(d => d.Id)
            .HasColumnName("id")
            .IsRequired();

        builder.Property(d => d.Identifier)
            .HasColumnName("identifier")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(d => d.Name)
            .HasColumnName("name")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(d => d.CNPJ)
            .HasColumnName("cnpj")
            .HasMaxLength(14)
            .IsRequired()
            .HasConversion(
                v => v.Value,
                v => CNPJ.Create(v));

        builder.Property(d => d.DateOfBirth)
            .HasColumnName("date_of_birth")
            .IsRequired();

        builder.OwnsOne(d => d.CNH, cnh =>
        {
            cnh.Property(c => c.Number)
                .HasColumnName("cnh_number")
                .HasMaxLength(11)
                .IsRequired();

            cnh.Property(c => c.Type)
                .HasColumnName("cnh_type")
                .HasConversion<string>()
                .IsRequired();

            cnh.Property(c => c.ImagePath)
                .HasColumnName("cnh_image_path")
                .HasMaxLength(500);
        });

        builder.Property(d => d.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(d => d.UpdatedAt)
            .HasColumnName("updated_at");

        // Indexes
        builder.HasIndex(d => d.CNPJ)
            .IsUnique()
            .HasDatabaseName("ix_delivery_drivers_cnpj");

        builder.HasIndex("CNH_Number")
            .IsUnique()
            .HasDatabaseName("ix_delivery_drivers_cnh_number");

        builder.HasIndex(d => d.Identifier)
            .HasDatabaseName("ix_delivery_drivers_identifier");
    }
}

