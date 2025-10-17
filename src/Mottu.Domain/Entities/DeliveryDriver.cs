using Mottu.Domain.Common;
using Mottu.Domain.Enums;
using Mottu.Domain.Exceptions;
using Mottu.Domain.ValueObjects;

namespace Mottu.Domain.Entities;

public class DeliveryDriver : Entity
{
    public string Identifier { get; private set; }
    public string Name { get; private set; }
    public CNPJ CNPJ { get; private set; }
    public DateTime DateOfBirth { get; private set; }
    public CNH CNH { get; private set; }

    private DeliveryDriver() : base() { }

    private DeliveryDriver(
        Guid id,
        string identifier,
        string name,
        CNPJ cnpj,
        DateTime dateOfBirth,
        CNH cnh) : base(id)
    {
        Identifier = identifier;
        Name = name;
        CNPJ = cnpj;
        DateOfBirth = dateOfBirth;
        CNH = cnh;
    }

    public static DeliveryDriver Create(
        string identifier,
        string name,
        string cnpj,
        DateTime dateOfBirth,
        string cnhNumber,
        CNHType cnhType,
        string? cnhImagePath = null)
    {
        if (string.IsNullOrWhiteSpace(identifier))
            throw new DomainException("INVALID_IDENTIFIER", "Identifier cannot be empty");

        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("INVALID_NAME", "Name cannot be empty");

        if (dateOfBirth >= DateTime.UtcNow)
            throw new DomainException("INVALID_DATE_OF_BIRTH", "Date of birth must be in the past");

        var age = DateTime.UtcNow.Year - dateOfBirth.Year;
        if (dateOfBirth.Date > DateTime.UtcNow.AddYears(-age))
            age--;

        if (age < 18)
            throw new DomainException("INVALID_AGE", "Driver must be at least 18 years old");

        var cnpjValue = CNPJ.Create(cnpj);
        var cnhValue = CNH.Create(cnhNumber, cnhType, cnhImagePath);

        return new DeliveryDriver(Guid.NewGuid(), identifier, name, cnpjValue, dateOfBirth, cnhValue);
    }

    public void UpdateCNHImage(string imagePath)
    {
        if (string.IsNullOrWhiteSpace(imagePath))
            throw new DomainException("INVALID_IMAGE_PATH", "Image path cannot be empty");

        CNH = CNH.UpdateImage(imagePath);
        UpdatedAt = DateTime.UtcNow;
    }

    public bool CanRentMotorcycle()
    {
        return CNH.Type == CNHType.A || CNH.Type == CNHType.AB;
    }
}

