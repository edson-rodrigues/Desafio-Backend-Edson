using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Mottu.Application.DTOs;
using Xunit;

namespace Mottu.IntegrationTests.Controllers;

public class RentalsControllerTests : IntegrationTestBase
{
    public RentalsControllerTests(WebApplicationFactory<Program> factory) : base(factory)
    {
    }

    [Fact]
    public async Task CreateRental_WithInvalidDriver_ShouldReturnBadRequest()
    {
        // Arrange
        var rental = new RentalDto
        {
            Entregador_id = Guid.NewGuid().ToString(),
            Moto_id = Guid.NewGuid().ToString(),
            Data_inicio = DateTime.UtcNow.Date.AddDays(1),
            Data_termino = DateTime.UtcNow.Date.AddDays(8),
            Data_previsao_termino = DateTime.UtcNow.Date.AddDays(8),
            Plano = 7
        };

        // Act
        var response = await Client.PostAsJsonAsync("/locacao", rental);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateRental_WithInvalidPlan_ShouldReturnBadRequest()
    {
        // Arrange
        var rental = new RentalDto
        {
            Entregador_id = Guid.NewGuid().ToString(),
            Moto_id = Guid.NewGuid().ToString(),
            Data_inicio = DateTime.UtcNow.Date.AddDays(1),
            Data_termino = DateTime.UtcNow.Date.AddDays(10),
            Data_previsao_termino = DateTime.UtcNow.Date.AddDays(10),
            Plano = 10 // Invalid plan
        };

        // Act
        var response = await Client.PostAsJsonAsync("/locacao", rental);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GetRentalCost_WithNonExistentRental_ShouldReturnNotFound()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();
        var returnDate = DateTime.UtcNow.Date.AddDays(7);

        // Act
        var response = await Client.GetAsync($"/locacao/{nonExistentId}/valor?data_devolucao={returnDate:yyyy-MM-dd}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetRentalCost_WithInvalidDate_ShouldReturnBadRequest()
    {
        // Arrange
        var rentalId = Guid.NewGuid();
        var invalidDate = "invalid-date";

        // Act
        var response = await Client.GetAsync($"/locacao/{rentalId}/valor?data_devolucao={invalidDate}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}

