using Mottu.Domain.Common;
using Mottu.Domain.Exceptions;

namespace Mottu.Domain.ValueObjects;

public sealed class Money : ValueObject
{
    public decimal Amount { get; private set; }
    public string Currency { get; private set; }

    private Money(decimal amount, string currency)
    {
        Amount = amount;
        Currency = currency;
    }

    public static Money Create(decimal amount, string currency = "BRL")
    {
        if (amount < 0)
            throw new DomainException("INVALID_AMOUNT", "Amount cannot be negative");

        if (string.IsNullOrWhiteSpace(currency))
            throw new DomainException("INVALID_CURRENCY", "Currency cannot be empty");

        return new Money(amount, currency);
    }

    public static Money Zero(string currency = "BRL") => new Money(0, currency);

    public Money Add(Money other)
    {
        if (Currency != other.Currency)
            throw new DomainException("CURRENCY_MISMATCH", "Cannot add money with different currencies");

        return new Money(Amount + other.Amount, Currency);
    }

    public Money Subtract(Money other)
    {
        if (Currency != other.Currency)
            throw new DomainException("CURRENCY_MISMATCH", "Cannot subtract money with different currencies");

        return new Money(Amount - other.Amount, Currency);
    }

    public Money Multiply(decimal multiplier)
    {
        return new Money(Amount * multiplier, Currency);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Amount;
        yield return Currency;
    }

    public override string ToString() => $"{Currency} {Amount:N2}";

    public static implicit operator decimal(Money money) => money.Amount;
}

