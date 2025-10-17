using MediatR;
using Microsoft.AspNetCore.Mvc;
using Mottu.Application.Commands.Motorcycles;
using Mottu.Application.DTOs;
using Mottu.Application.Queries.Motorcycles;

namespace Mottu.API.Controllers;

[ApiController]
[Route("motos")]
[Produces("application/json")]
public class MotorcyclesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<MotorcyclesController> _logger;

    public MotorcyclesController(IMediator mediator, ILogger<MotorcyclesController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Cadastrar uma nova moto
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(object), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RegisterMotorcycle([FromBody] MotorcycleDto request)
    {
        var command = new RegisterMotorcycleCommand(
            request.Identificador!,
            request.Ano,
            request.Modelo!,
            request.Placa!);

        var result = await _mediator.Send(command);

        if (result.IsFailure)
        {
            return BadRequest(new { mensagem = result.Error.Message });
        }

        return Created($"/motos/{result.Value}", new { id = result.Value, mensagem = "Moto cadastrada com sucesso" });
    }

    /// <summary>
    /// Consultar motos existentes
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<MotorcycleDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMotorcycles([FromQuery] string? placa = null)
    {
        var query = new GetMotorcyclesQuery(placa);
        var result = await _mediator.Send(query);

        if (result.IsFailure)
        {
            return BadRequest(new { mensagem = result.Error.Message });
        }

        return Ok(result.Value);
    }

    /// <summary>
    /// Consultar moto por id
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(MotorcycleDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetMotorcycleById(string id)
    {
        var query = new GetMotorcycleByIdQuery(id);
        var result = await _mediator.Send(query);

        if (result.IsFailure)
        {
            return NotFound(new { mensagem = result.Error.Message });
        }

        return Ok(result.Value);
    }

    /// <summary>
    /// Modificar a placa de uma moto
    /// </summary>
    [HttpPut("{id}/placa")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateLicensePlate(string id, [FromBody] UpdatePlacaRequest request)
    {
        var command = new UpdateMotorcycleLicensePlateCommand(id, request.Placa!);
        var result = await _mediator.Send(command);

        if (result.IsFailure)
        {
            if (result.Error.Code == "NOT_FOUND")
                return NotFound(new { mensagem = result.Error.Message });

            return BadRequest(new { mensagem = result.Error.Message });
        }

        return Ok(new { mensagem = "Placa modificada com sucesso" });
    }

    /// <summary>
    /// Remover uma moto
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteMotorcycle(string id)
    {
        var command = new DeleteMotorcycleCommand(id);
        var result = await _mediator.Send(command);

        if (result.IsFailure)
        {
            if (result.Error.Code == "NOT_FOUND")
                return NotFound(new { mensagem = result.Error.Message });

            return BadRequest(new { mensagem = result.Error.Message });
        }

        return Ok(new { mensagem = "Moto removida com sucesso" });
    }
}

public record UpdatePlacaRequest(string Placa);

