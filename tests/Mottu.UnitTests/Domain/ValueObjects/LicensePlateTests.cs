using Mottu.Domain.Exceptions;
using Mottu.Domain.ValueObjects;
using Xunit;

namespace Mottu.UnitTests.Domain.ValueObjects;

public class LicensePlateTests
{
    [Theory]
    [InlineData("ABC1234")]
    [InlineData("XYZ9W88")]
    [InlineData("DEF5G67")]
    public void Create_WithValidPlate_ShouldSucceed(string plate)
    {
        // Act
        var licensePlate = LicensePlate.Create(plate);

        // Assert
        Assert.NotNull(licensePlate);
        Assert.Equal(plate, licensePlate.Value);
    }

    [Theory]
    [InlineData("ABC123")]    // Too short
    [InlineData("ABCD234")]   // Too many letters
    [InlineData("12ABC34")]   // Numbers at start
    [InlineData("")]          // Empty
    public void Create_WithInvalidPlate_ShouldThrowDomainException(string invalidPlate)
    {
        // Act & Assert
        Assert.Throws<DomainException>(() => LicensePlate.Create(invalidPlate));
    }

    [Fact]
    public void TwoPlates_WithSameValue_ShouldBeEqual()
    {
        // Arrange
        var plate1 = LicensePlate.Create("ABC1234");
        var plate2 = LicensePlate.Create("ABC1234");

        // Assert
        Assert.Equal(plate1, plate2);
    }
}

