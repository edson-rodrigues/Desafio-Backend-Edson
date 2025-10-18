using MediatR;
using Mottu.Shared.Results;

namespace Mottu.Application.Commands.Motorcycles;

public record RegisterMotorcycleCommand(
    int Ano,
    string Modelo,
    string Placa
) : IRequest<Result<string>>;

