using Mottu.Domain.Entities;
using Mottu.Domain.Exceptions;
using Xunit;

namespace Mottu.UnitTests.Domain.Entities;

public class MotorcycleTests
{
    [Fact]
    public void Create_WithValidData_ShouldSucceed()
    {
        // Arrange
        var identifier = "moto-001";
        var year = 2024;
        var model = "Honda CG 160";
        var licensePlate = "ABC1234";

        // Act
        var motorcycle = Motorcycle.Create(identifier, year, model, licensePlate);

        // Assert
        Assert.NotNull(motorcycle);
        Assert.Equal(identifier, motorcycle.Identifier);
        Assert.Equal(year, motorcycle.Year);
        Assert.Equal(model, motorcycle.Model);
        Assert.Equal(licensePlate, motorcycle.LicensePlate.Value);
    }

    [Fact]
    public void Create_WithEmptyIdentifier_ShouldThrowDomainException()
    {
        // Act & Assert
        Assert.Throws<DomainException>(() => 
            Motorcycle.Create("", 2024, "Model", "ABC1234"));
    }

    [Fact]
    public void Create_WithInvalidYear_ShouldThrowDomainException()
    {
        // Act & Assert
        Assert.Throws<DomainException>(() => 
            Motorcycle.Create("moto-001", 1800, "Model", "ABC1234"));
    }

    [Fact]
    public void UpdateLicensePlate_WithValidPlate_ShouldSucceed()
    {
        // Arrange
        var motorcycle = Motorcycle.Create("moto-001", 2024, "Model", "ABC1234");
        var newPlate = "XYZ9W88";

        // Act
        motorcycle.UpdateLicensePlate(newPlate);

        // Assert
        Assert.Equal(newPlate, motorcycle.LicensePlate.Value);
        Assert.NotNull(motorcycle.UpdatedAt);
    }

    [Fact]
    public void IsYear_WithMatchingYear_ShouldReturnTrue()
    {
        // Arrange
        var motorcycle = Motorcycle.Create("moto-001", 2024, "Model", "ABC1234");

        // Act
        var result = motorcycle.IsYear(2024);

        // Assert
        Assert.True(result);
    }
}

