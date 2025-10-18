using NetArchTest.Rules;
using Xunit;

namespace Mottu.ArchitectureTests;

public class LayerDependencyTests
{
    private const string DomainNamespace = "Mottu.Domain";
    private const string ApplicationNamespace = "Mottu.Application";
    private const string InfrastructureNamespace = "Mottu.Infrastructure";
    private const string ApiNamespace = "Mottu.API";

    [Fact]
    public void Domain_Should_Not_HaveDependencyOnOtherProjects()
    {
        // Arrange
        var assembly = typeof(Mottu.Domain.Entities.Motorcycle).Assembly;

        // Act
        var result = Types.InAssembly(assembly)
            .ShouldNot()
            .HaveDependencyOnAny(ApplicationNamespace, InfrastructureNamespace, ApiNamespace)
            .GetResult();

        // Assert
        Assert.True(result.IsSuccessful, "Domain layer should not depend on Application, Infrastructure or API layers");
    }

    [Fact]
    public void Application_Should_Not_HaveDependencyOnInfrastructureOrApi()
    {
        // Arrange
        var assembly = typeof(Mottu.Application.Commands.Motorcycles.RegisterMotorcycleCommand).Assembly;

        // Act
        var result = Types.InAssembly(assembly)
            .ShouldNot()
            .HaveDependencyOnAny(InfrastructureNamespace, ApiNamespace)
            .GetResult();

        // Assert
        Assert.True(result.IsSuccessful, "Application layer should not depend on Infrastructure or API layers");
    }

    [Fact]
    public void Infrastructure_Should_Not_HaveDependencyOnApi()
    {
        // Arrange
        var assembly = typeof(Mottu.Infrastructure.Persistence.PostgreSQL.ApplicationDbContext).Assembly;

        // Act
        var result = Types.InAssembly(assembly)
            .ShouldNot()
            .HaveDependencyOn(ApiNamespace)
            .GetResult();

        // Assert
        Assert.True(result.IsSuccessful, "Infrastructure layer should not depend on API layer");
    }

    [Fact]
    public void Controllers_Should_HaveSuffixController()
    {
        // Arrange
        var assembly = typeof(Mottu.API.Controllers.MotorcyclesController).Assembly;

        // Act
        var result = Types.InAssembly(assembly)
            .That()
            .ResideInNamespace("Mottu.API.Controllers")
            .Should()
            .HaveNameEndingWith("Controller")
            .GetResult();

        // Assert
        Assert.True(result.IsSuccessful, "All classes in Controllers namespace should end with 'Controller'");
    }
}

