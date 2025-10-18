using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Mottu.Application.DTOs;
using Xunit;

namespace Mottu.IntegrationTests.Controllers;

public class MotorcyclesControllerTests : IntegrationTestBase
{
    public MotorcyclesControllerTests(WebApplicationFactory<Program> factory) : base(factory)
    {
    }

    [Fact]
    public async Task RegisterMotorcycle_WithValidData_ShouldReturnCreated()
    {
        // Arrange
        var motorcycle = new MotorcycleDto
        {
            Identificador = $"test-moto-{Guid.NewGuid()}",
            Ano = 2024,
            Modelo = "Honda CG 160",
            Placa = $"ABC{Random.Shared.Next(1000, 9999)}"
        };

        // Act
        var response = await Client.PostAsJsonAsync("/motos", motorcycle);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var content = await response.Content.ReadAsStringAsync();
        content.Should().Contain("id");
        content.Should().Contain("mensagem");
    }

    [Fact]
    public async Task RegisterMotorcycle_WithInvalidPlate_ShouldReturnBadRequest()
    {
        // Arrange
        var motorcycle = new MotorcycleDto
        {
            Identificador = "test-moto-invalid",
            Ano = 2024,
            Modelo = "Honda CG 160",
            Placa = "INVALID" // Invalid format
        };

        // Act
        var response = await Client.PostAsJsonAsync("/motos", motorcycle);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GetMotorcycles_ShouldReturnOk()
    {
        // Act
        var response = await Client.GetAsync("/motos");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var motorcycles = await response.Content.ReadFromJsonAsync<List<MotorcycleDto>>();
        motorcycles.Should().NotBeNull();
    }

    [Fact]
    public async Task GetMotorcycleById_WithNonExistentId_ShouldReturnNotFound()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act
        var response = await Client.GetAsync($"/motos/{nonExistentId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task UpdateLicensePlate_WithInvalidId_ShouldReturnNotFound()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();
        var updateRequest = new { Placa = "XYZ9W88" };

        // Act
        var response = await Client.PutAsJsonAsync($"/motos/{nonExistentId}/placa", updateRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteMotorcycle_WithNonExistentId_ShouldReturnNotFound()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act
        var response = await Client.DeleteAsync($"/motos/{nonExistentId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}

