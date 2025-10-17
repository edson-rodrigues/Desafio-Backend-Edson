using MediatR;
using Microsoft.Extensions.Logging;
using Mottu.Application.DTOs;
using Mottu.Domain.Exceptions;
using Mottu.Domain.Interfaces;
using Mottu.Shared.Results;

namespace Mottu.Application.Queries.Rentals;

public class GetRentalCostQueryHandler : IRequestHandler<GetRentalCostQuery, Result<RentalCostDto>>
{
    private readonly IRentalRepository _rentalRepository;
    private readonly ILogger<GetRentalCostQueryHandler> _logger;

    public GetRentalCostQueryHandler(
        IRentalRepository rentalRepository,
        ILogger<GetRentalCostQueryHandler> logger)
    {
        _rentalRepository = rentalRepository;
        _logger = logger;
    }

    public async Task<Result<RentalCostDto>> Handle(GetRentalCostQuery request, CancellationToken cancellationToken)
    {
        try
        {
            if (!Guid.TryParse(request.RentalId, out var rentalId))
            {
                return Result.Failure<RentalCostDto>(Error.BadRequest("ID da locação inválido"));
            }

            var rental = await _rentalRepository.GetByIdAsync(rentalId, cancellationToken);
            if (rental == null)
            {
                _logger.LogWarning("Rental not found: {RentalId}", rentalId);
                return Result.Failure<RentalCostDto>(Error.NotFound("Rental", rentalId));
            }

            var totalCost = rental.CalculateReturnCost(request.Data_devolucao);

            var dto = new RentalCostDto
            {
                Valor_total = totalCost
            };

            return Result.Success(dto);
        }
        catch (DomainException ex)
        {
            _logger.LogWarning(ex, "Domain validation failed for rental cost calculation");
            return Result.Failure<RentalCostDto>(Error.Create(ex.Code, ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calculating rental cost");
            return Result.Failure<RentalCostDto>(Error.Create("RENTAL_COST_CALCULATION_FAILED", "Erro ao calcular custo da locação"));
        }
    }
}

