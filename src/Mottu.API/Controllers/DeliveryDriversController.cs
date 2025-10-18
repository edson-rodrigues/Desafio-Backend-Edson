using MediatR;
using Microsoft.AspNetCore.Mvc;
using Mottu.Application.Commands.DeliveryDrivers;
using Mottu.Application.DTOs;

namespace Mottu.API.Controllers;

[ApiController]
[Route("entregadores")]
[Produces("application/json")]
public class DeliveryDriversController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<DeliveryDriversController> _logger;

    public DeliveryDriversController(IMediator mediator, ILogger<DeliveryDriversController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Cadastrar entregador
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(object), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RegisterDeliveryDriver([FromBody] DeliveryDriverDto request)
    {
        var command = new RegisterDeliveryDriverCommand(
            request.Nome!,
            request.Cnpj!,
            request.Data_nascimento,
            request.Numero_cnh!,
            request.Tipo_cnh!,
            null); // Imagem será enviada separadamente via endpoint de upload

        var result = await _mediator.Send(command);

        if (result.IsFailure)
        {
            return BadRequest(new { mensagem = result.Error.Message });
        }

        return Created($"/entregadores/{result.Value}", new { id = result.Value, mensagem = "Entregador cadastrado com sucesso" });
    }

    /// <summary>
    /// Enviar foto da CNH
    /// </summary>
    [HttpPost("{id}/cnh")]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UploadCNHImage(string id, IFormFile imagem_cnh)
    {
        if (imagem_cnh == null || imagem_cnh.Length == 0)
        {
            return BadRequest(new { mensagem = "Imagem da CNH é obrigatória" });
        }

        var extension = Path.GetExtension(imagem_cnh.FileName).ToLowerInvariant();
        if (extension != ".png" && extension != ".bmp")
        {
            return BadRequest(new { mensagem = "Formato de arquivo inválido. Apenas PNG e BMP são permitidos" });
        }

        using var stream = imagem_cnh.OpenReadStream();
        
        var command = new UploadCNHImageCommand(
            id,
            stream,
            imagem_cnh.FileName,
            imagem_cnh.ContentType);

        var result = await _mediator.Send(command);

        if (result.IsFailure)
        {
            if (result.Error.Code == "NOT_FOUND")
                return NotFound(new { mensagem = result.Error.Message });

            return BadRequest(new { mensagem = result.Error.Message });
        }

        return Ok(new { mensagem = "Imagem da CNH enviada com sucesso" });
    }
}

