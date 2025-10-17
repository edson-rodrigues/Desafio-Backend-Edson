using MediatR;
using Mottu.Application.DTOs;
using Mottu.Shared.Results;

namespace Mottu.Application.Queries.Rentals;

public record GetRentalCostQuery(
    string RentalId,
    DateTime Data_devolucao
) : IRequest<Result<RentalCostDto>>;

