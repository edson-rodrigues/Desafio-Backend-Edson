using MediatR;
using Microsoft.Extensions.Logging;
using Mottu.Application.Interfaces;
using Mottu.Domain.Entities;
using Mottu.Domain.Events;
using Mottu.Domain.Exceptions;
using Mottu.Domain.Interfaces;
using Mottu.Domain.Specifications;
using Mottu.Shared.Results;

namespace Mottu.Application.Commands.Rentals;

public class CreateRentalCommandHandler : IRequestHandler<CreateRentalCommand, Result<string>>
{
    private readonly IRentalRepository _rentalRepository;
    private readonly IMotorcycleRepository _motorcycleRepository;
    private readonly IDeliveryDriverRepository _driverRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEventPublisher _eventPublisher;
    private readonly ILogger<CreateRentalCommandHandler> _logger;

    public CreateRentalCommandHandler(
        IRentalRepository rentalRepository,
        IMotorcycleRepository motorcycleRepository,
        IDeliveryDriverRepository driverRepository,
        IUnitOfWork unitOfWork,
        IEventPublisher eventPublisher,
        ILogger<CreateRentalCommandHandler> logger)
    {
        _rentalRepository = rentalRepository;
        _motorcycleRepository = motorcycleRepository;
        _driverRepository = driverRepository;
        _unitOfWork = unitOfWork;
        _eventPublisher = eventPublisher;
        _logger = logger;
    }

    public async Task<Result<string>> Handle(CreateRentalCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (!Guid.TryParse(request.Moto_id, out var motorcycleId))
            {
                return Result.Failure<string>(Error.BadRequest("ID da moto inválido"));
            }

            if (!Guid.TryParse(request.Entregador_id, out var driverId))
            {
                return Result.Failure<string>(Error.BadRequest("ID do entregador inválido"));
            }

            // Validate motorcycle exists
            var motorcycle = await _motorcycleRepository.GetByIdAsync(motorcycleId, cancellationToken);
            if (motorcycle == null)
            {
                _logger.LogWarning("Motorcycle not found: {MotorcycleId}", motorcycleId);
                return Result.Failure<string>(Error.NotFound("Motorcycle", motorcycleId));
            }

            // Validate driver exists
            var driver = await _driverRepository.GetByIdAsync(driverId, cancellationToken);
            if (driver == null)
            {
                _logger.LogWarning("Driver not found: {DriverId}", driverId);
                return Result.Failure<string>(Error.NotFound("DeliveryDriver", driverId));
            }

            // Check if driver can rent motorcycle (must have CNH type A or AB)
            var canRentSpec = new CanRentMotorcycleSpecification();
            if (!canRentSpec.IsSatisfiedBy(driver))
            {
                _logger.LogWarning("Driver cannot rent motorcycle: {DriverId}, CNH Type: {CNHType}", 
                    driverId, driver.CNH.Type);
                return Result.Failure<string>(Error.BadRequest(
                    "Entregador deve possuir CNH tipo A ou AB para alugar uma moto"));
            }

            // Check if driver already has an active rental
            var hasActiveRental = await _rentalRepository.HasActiveRentalAsync(driverId, cancellationToken);
            if (hasActiveRental)
            {
                _logger.LogWarning("Driver already has an active rental: {DriverId}", driverId);
                return Result.Failure<string>(Error.Conflict("Entregador já possui uma locação ativa"));
            }

            // Create rental
            var rental = Rental.Create(motorcycleId, driverId, request.Plano);

            await _rentalRepository.AddAsync(rental, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Rental created successfully with ID: {RentalId}", rental.Id);

            // Publish event
            var domainEvent = new RentalCreatedEvent(
                rental.Id,
                rental.MotorcycleId,
                rental.DeliveryDriverId,
                rental.StartDate,
                rental.ExpectedEndDate,
                rental.Plan.DurationDays,
                rental.TotalCost);

            await _eventPublisher.PublishAsync(domainEvent, cancellationToken);

            return Result.Success(rental.Id.ToString());
        }
        catch (DomainException ex)
        {
            _logger.LogWarning(ex, "Domain validation failed for rental creation");
            return Result.Failure<string>(Error.Create(ex.Code, ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating rental");
            return Result.Failure<string>(Error.Create("RENTAL_CREATION_FAILED", "Erro ao criar locação"));
        }
    }
}

