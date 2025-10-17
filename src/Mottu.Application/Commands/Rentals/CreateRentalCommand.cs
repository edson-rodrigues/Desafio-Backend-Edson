using MediatR;
using Mottu.Shared.Results;

namespace Mottu.Application.Commands.Rentals;

public record CreateRentalCommand(
    string Entregador_id,
    string Moto_id,
    DateTime Data_inicio,
    DateTime Data_termino,
    DateTime Data_previsao_termino,
    int Plano
) : IRequest<Result<string>>;

