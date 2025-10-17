using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mottu.Domain.Entities;
using Mottu.Domain.Enums;
using Mottu.Domain.ValueObjects;

namespace Mottu.Infrastructure.Persistence.PostgreSQL.Configurations;

public class RentalConfiguration : IEntityTypeConfiguration<Rental>
{
    public void Configure(EntityTypeBuilder<Rental> builder)
    {
        builder.ToTable("rentals");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.Id)
            .HasColumnName("id")
            .IsRequired();

        builder.Property(r => r.MotorcycleId)
            .HasColumnName("motorcycle_id")
            .IsRequired();

        builder.Property(r => r.DeliveryDriverId)
            .HasColumnName("delivery_driver_id")
            .IsRequired();

        builder.Property(r => r.StartDate)
            .HasColumnName("start_date")
            .IsRequired();

        builder.Property(r => r.EndDate)
            .HasColumnName("end_date")
            .IsRequired();

        builder.Property(r => r.ExpectedEndDate)
            .HasColumnName("expected_end_date")
            .IsRequired();

        builder.Property(r => r.ActualReturnDate)
            .HasColumnName("actual_return_date");

        builder.OwnsOne(r => r.Plan, plan =>
        {
            plan.Property(p => p.DurationDays)
                .HasColumnName("plan_duration_days")
                .IsRequired();

            plan.Property(p => p.DailyCost)
                .HasColumnName("plan_daily_cost")
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            plan.Property(p => p.PenaltyPercentage)
                .HasColumnName("plan_penalty_percentage")
                .HasColumnType("decimal(5,2)")
                .IsRequired();
        });

        builder.Property(r => r.TotalCost)
            .HasColumnName("total_cost")
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(r => r.Status)
            .HasColumnName("status")
            .HasConversion<string>()
            .IsRequired();

        builder.Property(r => r.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(r => r.UpdatedAt)
            .HasColumnName("updated_at");

        // Relationships
        builder.HasOne(r => r.Motorcycle)
            .WithMany()
            .HasForeignKey(r => r.MotorcycleId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(r => r.DeliveryDriver)
            .WithMany()
            .HasForeignKey(r => r.DeliveryDriverId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        builder.HasIndex(r => r.MotorcycleId)
            .HasDatabaseName("ix_rentals_motorcycle_id");

        builder.HasIndex(r => r.DeliveryDriverId)
            .HasDatabaseName("ix_rentals_delivery_driver_id");

        builder.HasIndex(r => r.Status)
            .HasDatabaseName("ix_rentals_status");
    }
}

