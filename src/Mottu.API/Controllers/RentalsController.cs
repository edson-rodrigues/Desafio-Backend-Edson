using MediatR;
using Microsoft.AspNetCore.Mvc;
using Mottu.Application.Commands.Rentals;
using Mottu.Application.DTOs;
using Mottu.Application.Queries.Rentals;

namespace Mottu.API.Controllers;

[ApiController]
[Route("locacao")]
[Produces("application/json")]
public class RentalsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<RentalsController> _logger;

    public RentalsController(IMediator mediator, ILogger<RentalsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Alugar uma moto
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(object), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateRental([FromBody] RentalDto request)
    {
        var command = new CreateRentalCommand(
            request.Entregador_id!,
            request.Moto_id!,
            request.Data_inicio,
            request.Data_termino,
            request.Data_previsao_termino,
            request.Plano);

        var result = await _mediator.Send(command);

        if (result.IsFailure)
        {
            return BadRequest(new { mensagem = result.Error.Message });
        }

        return Created($"/locacao/{result.Value}", new { id = result.Value, mensagem = "Locação criada com sucesso" });
    }

    /// <summary>
    /// Consultar valor total da locação
    /// </summary>
    [HttpGet("{id}/valor")]
    [ProducesResponseType(typeof(RentalCostDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetRentalCost(string id, [FromQuery] DateTime data_devolucao)
    {
        var query = new GetRentalCostQuery(id, data_devolucao);
        var result = await _mediator.Send(query);

        if (result.IsFailure)
        {
            if (result.Error.Code == "NOT_FOUND")
                return NotFound(new { mensagem = result.Error.Message });

            return BadRequest(new { mensagem = result.Error.Message });
        }

        return Ok(result.Value);
    }
}

