using MediatR;
using Mottu.Application.DTOs;
using Mottu.Shared.Results;

namespace Mottu.Application.Queries.Motorcycles;

public record GetMotorcyclesQuery(string? Placa = null) : IRequest<Result<IEnumerable<MotorcycleDto>>>;

