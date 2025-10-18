using NetArchTest.Rules;
using Xunit;

namespace Mottu.ArchitectureTests;

public class ImplementationTests
{
    [Fact]
    public void Entities_Should_InheritFromEntity()
    {
        // Arrange
        var assembly = typeof(Mottu.Domain.Entities.Motorcycle).Assembly;

        // Act
        var result = Types.InAssembly(assembly)
            .That()
            .ResideInNamespace("Mottu.Domain.Entities")
            .Should()
            .Inherit(typeof(Mottu.Domain.Common.Entity))
            .GetResult();

        // Assert
        Assert.True(result.IsSuccessful, "All entities should inherit from Entity base class");
    }

    [Fact]
    public void ValueObjects_Should_InheritFromValueObject()
    {
        // Arrange
        var assembly = typeof(Mottu.Domain.ValueObjects.LicensePlate).Assembly;

        // Act
        var result = Types.InAssembly(assembly)
            .That()
            .ResideInNamespace("Mottu.Domain.ValueObjects")
            .Should()
            .Inherit(typeof(Mottu.Domain.Common.ValueObject))
            .GetResult();

        // Assert
        Assert.True(result.IsSuccessful, "All value objects should inherit from ValueObject base class");
    }

    [Fact]
    public void Validators_Should_HaveValidatorSuffix()
    {
        // Arrange
        var assembly = typeof(Mottu.Application.Validators.RegisterMotorcycleCommandValidator).Assembly;

        // Act
        var result = Types.InAssembly(assembly)
            .That()
            .ResideInNamespace("Mottu.Application.Validators")
            .Should()
            .HaveNameEndingWith("Validator")
            .GetResult();

        // Assert
        Assert.True(result.IsSuccessful, "Validator classes should end with 'Validator'");
    }

    [Fact]
    public void Controllers_Should_InheritFromControllerBase()
    {
        // Arrange
        var assembly = typeof(Mottu.API.Controllers.MotorcyclesController).Assembly;

        // Act
        var result = Types.InAssembly(assembly)
            .That()
            .ResideInNamespace("Mottu.API.Controllers")
            .Should()
            .Inherit(typeof(Microsoft.AspNetCore.Mvc.ControllerBase))
            .GetResult();

        // Assert
        Assert.True(result.IsSuccessful, "All controllers should inherit from ControllerBase");
    }

    [Fact]
    public void Repositories_Should_ImplementIRepository()
    {
        // Arrange
        var assembly = typeof(Mottu.Infrastructure.Persistence.PostgreSQL.Repositories.MotorcycleRepository).Assembly;

        // Act
        var result = Types.InAssembly(assembly)
            .That()
            .ResideInNamespaceEndingWith("Repositories")
            .And()
            .HaveNameEndingWith("Repository")
            .Should()
            .ImplementInterface(typeof(Mottu.Domain.Interfaces.IMotorcycleRepository))
            .Or()
            .ImplementInterface(typeof(Mottu.Domain.Interfaces.IDeliveryDriverRepository))
            .Or()
            .ImplementInterface(typeof(Mottu.Domain.Interfaces.IRentalRepository))
            .GetResult();

        // Assert
        Assert.True(result.IsSuccessful, "Repository implementations should implement their respective interfaces");
    }
}

