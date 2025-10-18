using Mottu.Domain.Common;
using Mottu.Domain.Exceptions;
using Mottu.Domain.ValueObjects;

namespace Mottu.Domain.Entities;

public class Motorcycle : Entity
{
    public int Year { get; private set; }
    public string Model { get; private set; }
    public LicensePlate LicensePlate { get; private set; }

    private Motorcycle() : base() { }

    private Motorcycle(Guid id, int year, string model, LicensePlate licensePlate) : base(id)
    {
        Year = year;
        Model = model;
        LicensePlate = licensePlate;
    }

    public static Motorcycle Create(int year, string model, string licensePlate)
    {
        if (year < 1900 || year > DateTime.UtcNow.Year + 1)
            throw new DomainException("INVALID_YEAR", $"Year must be between 1900 and {DateTime.UtcNow.Year + 1}");

        if (string.IsNullOrWhiteSpace(model))
            throw new DomainException("INVALID_MODEL", "Model cannot be empty");

        var plate = LicensePlate.Create(licensePlate);

        return new Motorcycle(Guid.NewGuid(), year, model, plate);
    }

    public void UpdateLicensePlate(string newLicensePlate)
    {
        LicensePlate = LicensePlate.Create(newLicensePlate);
        UpdatedAt = DateTime.UtcNow;
    }

    public bool IsYear(int year) => Year == year;
}

