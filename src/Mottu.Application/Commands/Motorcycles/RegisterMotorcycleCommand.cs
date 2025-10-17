using MediatR;
using Mottu.Shared.Results;

namespace Mottu.Application.Commands.Motorcycles;

public record RegisterMotorcycleCommand(
    string Identificador,
    int Ano,
    string Modelo,
    string Placa
) : IRequest<Result<string>>;

