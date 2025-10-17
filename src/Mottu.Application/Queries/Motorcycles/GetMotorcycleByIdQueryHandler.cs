using MediatR;
using Microsoft.Extensions.Logging;
using Mottu.Application.DTOs;
using Mottu.Domain.Interfaces;
using Mottu.Shared.Results;

namespace Mottu.Application.Queries.Motorcycles;

public class GetMotorcycleByIdQueryHandler : IRequestHandler<GetMotorcycleByIdQuery, Result<MotorcycleDto>>
{
    private readonly IMotorcycleRepository _motorcycleRepository;
    private readonly ILogger<GetMotorcycleByIdQueryHandler> _logger;

    public GetMotorcycleByIdQueryHandler(
        IMotorcycleRepository motorcycleRepository,
        ILogger<GetMotorcycleByIdQueryHandler> logger)
    {
        _motorcycleRepository = motorcycleRepository;
        _logger = logger;
    }

    public async Task<Result<MotorcycleDto>> Handle(GetMotorcycleByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            if (!Guid.TryParse(request.Id, out var motorcycleId))
            {
                return Result.Failure<MotorcycleDto>(Error.BadRequest("ID inválido"));
            }

            var motorcycle = await _motorcycleRepository.GetByIdAsync(motorcycleId, cancellationToken);
            if (motorcycle == null)
            {
                _logger.LogWarning("Motorcycle not found: {MotorcycleId}", motorcycleId);
                return Result.Failure<MotorcycleDto>(Error.NotFound("Motorcycle", motorcycleId));
            }

            var dto = new MotorcycleDto
            {
                Identificador = motorcycle.Identifier,
                Ano = motorcycle.Year,
                Modelo = motorcycle.Model,
                Placa = motorcycle.LicensePlate
            };

            return Result.Success(dto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving motorcycle");
            return Result.Failure<MotorcycleDto>(
                Error.Create("MOTORCYCLE_RETRIEVAL_FAILED", "Erro ao consultar moto"));
        }
    }
}

