using MediatR;
using Microsoft.Extensions.Logging;
using Mottu.Application.DTOs;
using Mottu.Domain.Interfaces;
using Mottu.Shared.Results;

namespace Mottu.Application.Queries.Motorcycles;

public class GetMotorcyclesQueryHandler : IRequestHandler<GetMotorcyclesQuery, Result<IEnumerable<MotorcycleDto>>>
{
    private readonly IMotorcycleRepository _motorcycleRepository;
    private readonly ILogger<GetMotorcyclesQueryHandler> _logger;

    public GetMotorcyclesQueryHandler(
        IMotorcycleRepository motorcycleRepository,
        ILogger<GetMotorcyclesQueryHandler> logger)
    {
        _motorcycleRepository = motorcycleRepository;
        _logger = logger;
    }

    public async Task<Result<IEnumerable<MotorcycleDto>>> Handle(GetMotorcyclesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var motorcycles = await _motorcycleRepository.GetAllAsync(request.Placa, cancellationToken);

            var dtos = motorcycles.Select(m => new MotorcycleDto
            {
                Identificador = m.Identifier,
                Ano = m.Year,
                Modelo = m.Model,
                Placa = m.LicensePlate
            });

            return Result.Success(dtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving motorcycles");
            return Result.Failure<IEnumerable<MotorcycleDto>>(
                Error.Create("MOTORCYCLES_RETRIEVAL_FAILED", "Erro ao consultar motos"));
        }
    }
}

