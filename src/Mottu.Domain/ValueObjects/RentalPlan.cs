using Mottu.Domain.Common;
using Mottu.Domain.Exceptions;

namespace Mottu.Domain.ValueObjects;

public sealed class RentalPlan : ValueObject
{
    public int DurationDays { get; private set; }
    public decimal DailyCost { get; private set; }
    public decimal PenaltyPercentage { get; private set; }
    public decimal TotalCost => DurationDays * DailyCost;

    private RentalPlan(int durationDays, decimal dailyCost, decimal penaltyPercentage)
    {
        DurationDays = durationDays;
        DailyCost = dailyCost;
        PenaltyPercentage = penaltyPercentage;
    }

    public static RentalPlan Create(int days)
    {
        return days switch
        {
            7 => new RentalPlan(7, 30.00m, 0.20m),
            15 => new RentalPlan(15, 28.00m, 0.40m),
            30 => new RentalPlan(30, 22.00m, 0.00m),
            45 => new RentalPlan(45, 20.00m, 0.00m),
            50 => new RentalPlan(50, 18.00m, 0.00m),
            _ => throw new DomainException("INVALID_RENTAL_PLAN", $"Invalid rental plan duration: {days} days. Valid options are 7, 15, 30, 45, or 50 days.")
        };
    }

    public decimal CalculatePenalty(int unusedDays)
    {
        if (unusedDays <= 0)
            return 0;

        return unusedDays * DailyCost * PenaltyPercentage;
    }

    public decimal CalculateLateFee(int extraDays)
    {
        if (extraDays <= 0)
            return 0;

        const decimal lateFeePerDay = 50.00m;
        return extraDays * lateFeePerDay;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return DurationDays;
        yield return DailyCost;
        yield return PenaltyPercentage;
    }

    public override string ToString() => $"{DurationDays} days - R${DailyCost}/day";
}

