using MediatR;
using Mottu.Shared.Results;

namespace Mottu.Application.Commands.DeliveryDrivers;

public record RegisterDeliveryDriverCommand(
    string Nome,
    string Cnpj,
    DateTime Data_nascimento,
    string Numero_cnh,
    string Tipo_cnh,
    string? Imagem_cnh = null
) : IRequest<Result<string>>;

