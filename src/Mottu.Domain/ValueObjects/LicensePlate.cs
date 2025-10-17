using Mottu.Domain.Common;
using Mottu.Domain.Exceptions;
using System.Text.RegularExpressions;

namespace Mottu.Domain.ValueObjects;

public sealed class LicensePlate : ValueObject
{
    private static readonly Regex BrazilianPlateRegex = new(@"^[A-Z]{3}[0-9][A-Z0-9][0-9]{2}$", RegexOptions.Compiled);

    public string Value { get; private set; }

    private LicensePlate(string value)
    {
        Value = value;
    }

    public static LicensePlate Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("INVALID_LICENSE_PLATE", "License plate cannot be empty");

        var normalizedValue = value.ToUpperInvariant().Replace("-", "").Replace(" ", "");

        if (!BrazilianPlateRegex.IsMatch(normalizedValue))
            throw new DomainException("INVALID_LICENSE_PLATE", "License plate format is invalid");

        return new LicensePlate(normalizedValue);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;

    public static implicit operator string(LicensePlate licensePlate) => licensePlate.Value;
}

