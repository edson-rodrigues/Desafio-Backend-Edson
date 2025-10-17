using Mottu.Domain.Exceptions;
using Mottu.Domain.ValueObjects;
using Xunit;

namespace Mottu.UnitTests.Domain.ValueObjects;

public class RentalPlanTests
{
    [Theory]
    [InlineData(7, 30.00)]
    [InlineData(15, 28.00)]
    [InlineData(30, 22.00)]
    [InlineData(45, 20.00)]
    [InlineData(50, 18.00)]
    public void Create_WithValidDuration_ShouldHaveCorrectDailyCost(int days, double expectedCost)
    {
        // Act
        var plan = RentalPlan.Create(days);

        // Assert
        Assert.Equal(days, plan.DurationDays);
        Assert.Equal((decimal)expectedCost, plan.DailyCost);
    }

    [Theory]
    [InlineData(7, 0.20)]
    [InlineData(15, 0.40)]
    [InlineData(30, 0.00)]
    [InlineData(45, 0.00)]
    [InlineData(50, 0.00)]
    public void Create_ShouldHaveCorrectPenaltyPercentage(int days, double expectedPenalty)
    {
        // Act
        var plan = RentalPlan.Create(days);

        // Assert
        Assert.Equal((decimal)expectedPenalty, plan.PenaltyPercentage);
    }

    [Fact]
    public void Create_WithInvalidDuration_ShouldThrowDomainException()
    {
        // Act & Assert
        Assert.Throws<DomainException>(() => RentalPlan.Create(10));
    }

    [Fact]
    public void CalculatePenalty_ForSevenDaysPlan_ShouldCalculateCorrectly()
    {
        // Arrange
        var plan = RentalPlan.Create(7);
        int unusedDays = 2;

        // Act
        var penalty = plan.CalculatePenalty(unusedDays);

        // Assert
        // 2 days * R$30 * 20% = R$12
        Assert.Equal(12.00m, penalty);
    }

    [Fact]
    public void CalculateLateFee_ShouldChargeR50PerDay()
    {
        // Arrange
        var plan = RentalPlan.Create(7);
        int extraDays = 3;

        // Act
        var lateFee = plan.CalculateLateFee(extraDays);

        // Assert
        // 3 days * R$50 = R$150
        Assert.Equal(150.00m, lateFee);
    }
}

