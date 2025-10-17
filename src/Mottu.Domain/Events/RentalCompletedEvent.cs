using Mottu.Domain.Common;

namespace Mottu.Domain.Events;

public class RentalCompletedEvent : IDomainEvent
{
    public Guid EventId { get; }
    public DateTime OccurredOn { get; }
    public Guid RentalId { get; }
    public DateTime ReturnDate { get; }
    public decimal FinalCost { get; }

    public RentalCompletedEvent(
        Guid rentalId,
        DateTime returnDate,
        decimal finalCost)
    {
        EventId = Guid.NewGuid();
        OccurredOn = DateTime.UtcNow;
        RentalId = rentalId;
        ReturnDate = returnDate;
        FinalCost = finalCost;
    }
}

