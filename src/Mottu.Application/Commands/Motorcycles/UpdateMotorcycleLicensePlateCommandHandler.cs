using MediatR;
using Microsoft.Extensions.Logging;
using Mottu.Domain.Exceptions;
using Mottu.Domain.Interfaces;
using Mottu.Shared.Results;

namespace Mottu.Application.Commands.Motorcycles;

public class UpdateMotorcycleLicensePlateCommandHandler : IRequestHandler<UpdateMotorcycleLicensePlateCommand, Result>
{
    private readonly IMotorcycleRepository _motorcycleRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateMotorcycleLicensePlateCommandHandler> _logger;

    public UpdateMotorcycleLicensePlateCommandHandler(
        IMotorcycleRepository motorcycleRepository,
        IUnitOfWork unitOfWork,
        ILogger<UpdateMotorcycleLicensePlateCommandHandler> logger)
    {
        _motorcycleRepository = motorcycleRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result> Handle(UpdateMotorcycleLicensePlateCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (!Guid.TryParse(request.Id, out var motorcycleId))
            {
                return Result.Failure(Error.BadRequest("ID inválido"));
            }

            var motorcycle = await _motorcycleRepository.GetByIdAsync(motorcycleId, cancellationToken);
            if (motorcycle == null)
            {
                _logger.LogWarning("Motorcycle not found: {MotorcycleId}", motorcycleId);
                return Result.Failure(Error.NotFound("Motorcycle", motorcycleId));
            }

            // Check if new plate already exists
            var existingByPlate = await _motorcycleRepository.GetByLicensePlateAsync(request.Placa, cancellationToken);
            if (existingByPlate != null && existingByPlate.Id != motorcycleId)
            {
                _logger.LogWarning("Attempted to update motorcycle with duplicate license plate: {LicensePlate}", request.Placa);
                return Result.Failure(Error.Conflict("Placa já cadastrada"));
            }

            motorcycle.UpdateLicensePlate(request.Placa);

            await _motorcycleRepository.UpdateAsync(motorcycle, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Motorcycle license plate updated successfully: {MotorcycleId}", motorcycleId);

            return Result.Success();
        }
        catch (DomainException ex)
        {
            _logger.LogWarning(ex, "Domain validation failed for license plate update");
            return Result.Failure(Error.Create(ex.Code, ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating motorcycle license plate");
            return Result.Failure(Error.Create("LICENSE_PLATE_UPDATE_FAILED", "Erro ao atualizar placa"));
        }
    }
}

