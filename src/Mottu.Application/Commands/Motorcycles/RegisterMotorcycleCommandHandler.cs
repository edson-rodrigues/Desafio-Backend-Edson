using MediatR;
using Microsoft.Extensions.Logging;
using Mottu.Application.Interfaces;
using Mottu.Domain.Entities;
using Mottu.Domain.Events;
using Mottu.Domain.Exceptions;
using Mottu.Domain.Interfaces;
using Mottu.Shared.Results;

namespace Mottu.Application.Commands.Motorcycles;

public class RegisterMotorcycleCommandHandler : IRequestHandler<RegisterMotorcycleCommand, Result<string>>
{
    private readonly IMotorcycleRepository _motorcycleRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEventPublisher _eventPublisher;
    private readonly ILogger<RegisterMotorcycleCommandHandler> _logger;

    public RegisterMotorcycleCommandHandler(
        IMotorcycleRepository motorcycleRepository,
        IUnitOfWork unitOfWork,
        IEventPublisher eventPublisher,
        ILogger<RegisterMotorcycleCommandHandler> logger)
    {
        _motorcycleRepository = motorcycleRepository;
        _unitOfWork = unitOfWork;
        _eventPublisher = eventPublisher;
        _logger = logger;
    }

    public async Task<Result<string>> Handle(RegisterMotorcycleCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Check if license plate already exists
            var existingByPlate = await _motorcycleRepository.ExistsByLicensePlateAsync(request.Placa, cancellationToken);
            if (existingByPlate)
            {
                _logger.LogWarning("Attempted to register motorcycle with duplicate license plate: {LicensePlate}", request.Placa);
                return Result.Failure<string>(Error.Conflict("Placa já cadastrada"));
            }

            // Create motorcycle entity
            var motorcycle = Motorcycle.Create(
                request.Ano,
                request.Modelo,
                request.Placa);

            // Save to database
            await _motorcycleRepository.AddAsync(motorcycle, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Motorcycle registered successfully with ID: {MotorcycleId}", motorcycle.Id);

            // Publish domain event
            var domainEvent = new MotorcycleRegisteredEvent(
                motorcycle.Id,
                motorcycle.LicensePlate.Value, // Use license plate as identifier
                motorcycle.Year,
                motorcycle.Model,
                motorcycle.LicensePlate);

            await _eventPublisher.PublishAsync(domainEvent, cancellationToken);

            return Result.Success(motorcycle.Id.ToString());
        }
        catch (DomainException ex)
        {
            _logger.LogWarning(ex, "Domain validation failed for motorcycle registration");
            return Result.Failure<string>(Error.Create(ex.Code, ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error registering motorcycle");
            return Result.Failure<string>(Error.Create("MOTORCYCLE_REGISTRATION_FAILED", "Erro ao cadastrar moto"));
        }
    }
}

