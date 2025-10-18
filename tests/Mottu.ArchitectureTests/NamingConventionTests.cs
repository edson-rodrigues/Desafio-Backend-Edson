using NetArchTest.Rules;
using Xunit;

namespace Mottu.ArchitectureTests;

public class NamingConventionTests
{
    [Fact]
    public void Entities_Should_NotHaveSuffix()
    {
        // Arrange
        var assembly = typeof(Mottu.Domain.Entities.Motorcycle).Assembly;

        // Act
        var result = Types.InAssembly(assembly)
            .That()
            .ResideInNamespace("Mottu.Domain.Entities")
            .Should()
            .NotHaveNameEndingWith("Entity")
            .GetResult();

        // Assert
        Assert.True(result.IsSuccessful, "Entities should not have 'Entity' suffix");
    }

    [Fact]
    public void Repositories_Should_HaveInterfaceStartingWithI()
    {
        // Arrange
        var assembly = typeof(Mottu.Domain.Interfaces.IMotorcycleRepository).Assembly;

        // Act
        var result = Types.InAssembly(assembly)
            .That()
            .ResideInNamespace("Mottu.Domain.Interfaces")
            .And()
            .AreInterfaces()
            .Should()
            .HaveNameStartingWith("I")
            .GetResult();

        // Assert
        Assert.True(result.IsSuccessful, "Repository interfaces should start with 'I'");
    }

    [Fact]
    public void Commands_Should_HaveCommandSuffix()
    {
        // Arrange
        var assembly = typeof(Mottu.Application.Commands.Motorcycles.RegisterMotorcycleCommand).Assembly;

        // Act
        var result = Types.InAssembly(assembly)
            .That()
            .ResideInNamespaceStartingWith("Mottu.Application.Commands")
            .And()
            .AreClasses()
            .Should()
            .HaveNameEndingWith("Command")
            .GetResult();

        // Assert
        Assert.True(result.IsSuccessful, "Command classes should end with 'Command'");
    }

    [Fact]
    public void Queries_Should_HaveQuerySuffix()
    {
        // Arrange
        var assembly = typeof(Mottu.Application.Queries.Motorcycles.GetMotorcyclesQuery).Assembly;

        // Act
        var result = Types.InAssembly(assembly)
            .That()
            .ResideInNamespaceStartingWith("Mottu.Application.Queries")
            .And()
            .AreClasses()
            .Should()
            .HaveNameEndingWith("Query")
            .GetResult();

        // Assert
        Assert.True(result.IsSuccessful, "Query classes should end with 'Query'");
    }

    [Fact]
    public void DTOs_Should_HaveDtoSuffix()
    {
        // Arrange
        var assembly = typeof(Mottu.Application.DTOs.MotorcycleDto).Assembly;

        // Act
        var result = Types.InAssembly(assembly)
            .That()
            .ResideInNamespace("Mottu.Application.DTOs")
            .Should()
            .HaveNameEndingWith("Dto")
            .GetResult();

        // Assert
        Assert.True(result.IsSuccessful, "DTO classes should end with 'Dto'");
    }
}

