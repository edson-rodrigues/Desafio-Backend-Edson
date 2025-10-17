using MediatR;
using Mottu.Application.DTOs;
using Mottu.Shared.Results;

namespace Mottu.Application.Queries.Motorcycles;

public record GetMotorcycleByIdQuery(string Id) : IRequest<Result<MotorcycleDto>>;

