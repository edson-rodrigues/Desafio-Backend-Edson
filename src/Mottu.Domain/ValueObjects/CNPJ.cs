using Mottu.Domain.Common;
using Mottu.Domain.Exceptions;

namespace Mottu.Domain.ValueObjects;

public sealed class CNPJ : ValueObject
{
    public string Value { get; private set; }

    private CNPJ(string value)
    {
        Value = value;
    }

    public static CNPJ Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("INVALID_CNPJ", "CNPJ cannot be empty");

        var cleanValue = value.Replace(".", "").Replace("/", "").Replace("-", "").Trim();

        if (cleanValue.Length != 14)
            throw new DomainException("INVALID_CNPJ", "CNPJ must have 14 digits");

        if (!IsValid(cleanValue))
            throw new DomainException("INVALID_CNPJ", "CNPJ is invalid");

        return new CNPJ(cleanValue);
    }

    private static bool IsValid(string cnpj)
    {
        if (!long.TryParse(cnpj, out _))
            return false;

        // Check if all digits are the same
        if (cnpj.Distinct().Count() == 1)
            return false;

        // Validate check digits
        int[] multiplier1 = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
        int[] multiplier2 = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

        string tempCnpj = cnpj.Substring(0, 12);
        int sum = 0;

        for (int i = 0; i < 12; i++)
            sum += int.Parse(tempCnpj[i].ToString()) * multiplier1[i];

        int remainder = sum % 11;
        remainder = remainder < 2 ? 0 : 11 - remainder;

        string digit = remainder.ToString();
        tempCnpj += digit;
        sum = 0;

        for (int i = 0; i < 13; i++)
            sum += int.Parse(tempCnpj[i].ToString()) * multiplier2[i];

        remainder = sum % 11;
        remainder = remainder < 2 ? 0 : 11 - remainder;

        digit += remainder.ToString();

        return cnpj.EndsWith(digit);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;

    public static implicit operator string(CNPJ cnpj) => cnpj.Value;
}

