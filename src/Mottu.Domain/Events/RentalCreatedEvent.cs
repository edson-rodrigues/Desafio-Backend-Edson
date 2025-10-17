using Mottu.Domain.Common;

namespace Mottu.Domain.Events;

public class RentalCreatedEvent : IDomainEvent
{
    public Guid EventId { get; }
    public DateTime OccurredOn { get; }
    public Guid RentalId { get; }
    public Guid MotorcycleId { get; }
    public Guid DeliveryDriverId { get; }
    public DateTime StartDate { get; }
    public DateTime ExpectedEndDate { get; }
    public int PlanDurationDays { get; }
    public decimal TotalCost { get; }

    public RentalCreatedEvent(
        Guid rentalId,
        Guid motorcycleId,
        Guid deliveryDriverId,
        DateTime startDate,
        DateTime expectedEndDate,
        int planDurationDays,
        decimal totalCost)
    {
        EventId = Guid.NewGuid();
        OccurredOn = DateTime.UtcNow;
        RentalId = rentalId;
        MotorcycleId = motorcycleId;
        DeliveryDriverId = deliveryDriverId;
        StartDate = startDate;
        ExpectedEndDate = expectedEndDate;
        PlanDurationDays = planDurationDays;
        TotalCost = totalCost;
    }
}

