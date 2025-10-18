using FluentValidation;
using Mottu.Application.Commands.Motorcycles;

namespace Mottu.Application.Validators;

public class RegisterMotorcycleCommandValidator : AbstractValidator<RegisterMotorcycleCommand>
{
    public RegisterMotorcycleCommandValidator()
    {
        RuleFor(x => x.Ano)
            .GreaterThan(1900).WithMessage("Ano deve ser maior que 1900")
            .LessThanOrEqualTo(DateTime.UtcNow.Year + 1).WithMessage($"Ano deve ser menor ou igual a {DateTime.UtcNow.Year + 1}");

        RuleFor(x => x.Modelo)
            .NotEmpty().WithMessage("Modelo é obrigatório");

        RuleFor(x => x.Placa)
            .NotEmpty().WithMessage("Placa é obrigatória")
            .Matches(@"^[A-Z]{3}[0-9][A-Z0-9][0-9]{2}$").WithMessage("Formato de placa inválido");
    }
}

