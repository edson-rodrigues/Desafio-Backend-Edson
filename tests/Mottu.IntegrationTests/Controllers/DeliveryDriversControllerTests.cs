using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Mottu.Application.DTOs;
using Xunit;

namespace Mottu.IntegrationTests.Controllers;

public class DeliveryDriversControllerTests : IntegrationTestBase
{
    public DeliveryDriversControllerTests(WebApplicationFactory<Program> factory) : base(factory)
    {
    }

    [Fact]
    public async Task RegisterDeliveryDriver_WithValidData_ShouldReturnCreated()
    {
        // Arrange
        var driver = new DeliveryDriverDto
        {
            Identificador = $"driver-{Guid.NewGuid()}",
            Nome = "João Silva",
            Cnpj = GenerateValidCNPJ(),
            Data_nascimento = new DateTime(1990, 5, 15),
            Numero_cnh = $"{Random.Shared.Next(10000000, 99999999)}",
            Tipo_cnh = "A"
        };

        // Act
        var response = await Client.PostAsJsonAsync("/entregadores", driver);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task RegisterDeliveryDriver_WithInvalidCNH_ShouldReturnBadRequest()
    {
        // Arrange
        var driver = new DeliveryDriverDto
        {
            Identificador = "driver-invalid",
            Nome = "João Silva",
            Cnpj = GenerateValidCNPJ(),
            Data_nascimento = new DateTime(1990, 5, 15),
            Numero_cnh = "12345678901",
            Tipo_cnh = "B" // Invalid for motorcycle rental
        };

        // Act
        var response = await Client.PostAsJsonAsync("/entregadores", driver);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task UploadCNH_WithNonExistentDriver_ShouldReturnNotFound()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();
        var content = new MultipartFormDataContent();
        var imageContent = new ByteArrayContent(new byte[] { 0x89, 0x50, 0x4E, 0x47 }); // PNG header
        imageContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/png");
        content.Add(imageContent, "imagem_cnh", "cnh.png");

        // Act
        var response = await Client.PostAsync($"/entregadores/{nonExistentId}/cnh", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    private static string GenerateValidCNPJ()
    {
        // Generate a random but valid CNPJ for testing
        var random = Random.Shared;
        var cnpj = new int[14];
        
        // Generate first 12 digits
        for (int i = 0; i < 12; i++)
        {
            cnpj[i] = random.Next(0, 10);
        }

        // Calculate first verification digit
        int sum = 0;
        int[] weights1 = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
        for (int i = 0; i < 12; i++)
        {
            sum += cnpj[i] * weights1[i];
        }
        cnpj[12] = (sum % 11) < 2 ? 0 : 11 - (sum % 11);

        // Calculate second verification digit
        sum = 0;
        int[] weights2 = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
        for (int i = 0; i < 13; i++)
        {
            sum += cnpj[i] * weights2[i];
        }
        cnpj[13] = (sum % 11) < 2 ? 0 : 11 - (sum % 11);

        return string.Join("", cnpj);
    }
}

