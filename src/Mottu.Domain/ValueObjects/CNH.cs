using Mottu.Domain.Common;
using Mottu.Domain.Enums;
using Mottu.Domain.Exceptions;

namespace Mottu.Domain.ValueObjects;

public sealed class CNH : ValueObject
{
    public string Number { get; private set; }
    public CNHType Type { get; private set; }
    public string? ImagePath { get; private set; }

    private CNH(string number, CNHType type, string? imagePath = null)
    {
        Number = number;
        Type = type;
        ImagePath = imagePath;
    }

    public static CNH Create(string number, CNHType type, string? imagePath = null)
    {
        if (string.IsNullOrWhiteSpace(number))
            throw new DomainException("INVALID_CNH_NUMBER", "CNH number cannot be empty");

        var cleanNumber = number.Replace(" ", "").Replace("-", "");

        if (cleanNumber.Length != 11)
            throw new DomainException("INVALID_CNH_NUMBER", "CNH number must have 11 digits");

        if (!long.TryParse(cleanNumber, out _))
            throw new DomainException("INVALID_CNH_NUMBER", "CNH number must contain only digits");

        if (!Enum.IsDefined(typeof(CNHType), type))
            throw new DomainException("INVALID_CNH_TYPE", "Invalid CNH type");

        return new CNH(cleanNumber, type, imagePath);
    }

    public CNH UpdateImage(string imagePath)
    {
        if (string.IsNullOrWhiteSpace(imagePath))
            throw new DomainException("INVALID_IMAGE_PATH", "Image path cannot be empty");

        return new CNH(Number, Type, imagePath);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Number;
        yield return Type;
    }

    public override string ToString() => $"{Number} - {Type}";
}

