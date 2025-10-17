using Mottu.Domain.Common;

namespace Mottu.Domain.Events;

public class MotorcycleRegisteredEvent : IDomainEvent
{
    public Guid EventId { get; }
    public DateTime OccurredOn { get; }
    public Guid MotorcycleId { get; }
    public string Identifier { get; }
    public int Year { get; }
    public string Model { get; }
    public string LicensePlate { get; }

    public MotorcycleRegisteredEvent(
        Guid motorcycleId,
        string identifier,
        int year,
        string model,
        string licensePlate)
    {
        EventId = Guid.NewGuid();
        OccurredOn = DateTime.UtcNow;
        MotorcycleId = motorcycleId;
        Identifier = identifier;
        Year = year;
        Model = model;
        LicensePlate = licensePlate;
    }
}

