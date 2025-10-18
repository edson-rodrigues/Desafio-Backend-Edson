using MediatR;
using Microsoft.Extensions.Logging;
using Mottu.Domain.Entities;
using Mottu.Domain.Enums;
using Mottu.Domain.Exceptions;
using Mottu.Domain.Interfaces;
using Mottu.Shared.Results;

namespace Mottu.Application.Commands.DeliveryDrivers;

public class RegisterDeliveryDriverCommandHandler : IRequestHandler<RegisterDeliveryDriverCommand, Result<string>>
{
    private readonly IDeliveryDriverRepository _driverRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<RegisterDeliveryDriverCommandHandler> _logger;

    public RegisterDeliveryDriverCommandHandler(
        IDeliveryDriverRepository driverRepository,
        IUnitOfWork unitOfWork,
        ILogger<RegisterDeliveryDriverCommandHandler> logger)
    {
        _driverRepository = driverRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<string>> Handle(RegisterDeliveryDriverCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Check if CNPJ already exists
            var existingByCnpj = await _driverRepository.ExistsByCNPJAsync(request.Cnpj, cancellationToken);
            if (existingByCnpj)
            {
                _logger.LogWarning("Attempted to register driver with duplicate CNPJ: {CNPJ}", request.Cnpj);
                return Result.Failure<string>(Error.Conflict("CNPJ já cadastrado"));
            }

            // Check if CNH number already exists
            var existingByCnh = await _driverRepository.ExistsByCNHNumberAsync(request.Numero_cnh, cancellationToken);
            if (existingByCnh)
            {
                _logger.LogWarning("Attempted to register driver with duplicate CNH: {CNH}", request.Numero_cnh);
                return Result.Failure<string>(Error.Conflict("Número da CNH já cadastrado"));
            }

            // Parse CNH type
            if (!Enum.TryParse<CNHType>(request.Tipo_cnh, true, out var cnhType))
            {
                return Result.Failure<string>(Error.BadRequest("Tipo de CNH inválido. Valores aceitos: A, B, AB"));
            }

            // Create delivery driver entity
            var driver = DeliveryDriver.Create(
                request.Nome,
                request.Cnpj,
                request.Data_nascimento,
                request.Numero_cnh,
                cnhType,
                request.Imagem_cnh);

            // Save to database
            await _driverRepository.AddAsync(driver, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Delivery driver registered successfully with ID: {DriverId}", driver.Id);

            return Result.Success(driver.Id.ToString());
        }
        catch (DomainException ex)
        {
            _logger.LogWarning(ex, "Domain validation failed for driver registration");
            return Result.Failure<string>(Error.Create(ex.Code, ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error registering delivery driver");
            return Result.Failure<string>(Error.Create("DRIVER_REGISTRATION_FAILED", "Erro ao cadastrar entregador"));
        }
    }
}

