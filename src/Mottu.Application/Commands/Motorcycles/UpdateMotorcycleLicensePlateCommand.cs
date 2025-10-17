using MediatR;
using Mottu.Shared.Results;

namespace Mottu.Application.Commands.Motorcycles;

public record UpdateMotorcycleLicensePlateCommand(
    string Id,
    string Placa
) : IRequest<Result>;

