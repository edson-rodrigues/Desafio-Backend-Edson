using MediatR;
using Mottu.Shared.Results;

namespace Mottu.Application.Commands.Motorcycles;

public record DeleteMotorcycleCommand(string Id) : IRequest<Result>;

