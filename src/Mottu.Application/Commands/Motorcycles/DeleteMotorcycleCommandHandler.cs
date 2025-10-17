using MediatR;
using Microsoft.Extensions.Logging;
using Mottu.Domain.Interfaces;
using Mottu.Shared.Results;

namespace Mottu.Application.Commands.Motorcycles;

public class DeleteMotorcycleCommandHandler : IRequestHandler<DeleteMotorcycleCommand, Result>
{
    private readonly IMotorcycleRepository _motorcycleRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteMotorcycleCommandHandler> _logger;

    public DeleteMotorcycleCommandHandler(
        IMotorcycleRepository motorcycleRepository,
        IUnitOfWork unitOfWork,
        ILogger<DeleteMotorcycleCommandHandler> logger)
    {
        _motorcycleRepository = motorcycleRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result> Handle(DeleteMotorcycleCommand request, CancellationToken cancellationToken)
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
                _logger.LogWarning("Motorcycle not found for deletion: {MotorcycleId}", motorcycleId);
                return Result.Failure(Error.NotFound("Motorcycle", motorcycleId));
            }

            // Check if motorcycle has rentals
            var hasRentals = await _motorcycleRepository.HasRentalsAsync(motorcycleId, cancellationToken);
            if (hasRentals)
            {
                _logger.LogWarning("Attempted to delete motorcycle with existing rentals: {MotorcycleId}", motorcycleId);
                return Result.Failure(Error.Conflict("Moto possui histórico de locações e não pode ser removida"));
            }

            await _motorcycleRepository.DeleteAsync(motorcycleId, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Motorcycle deleted successfully: {MotorcycleId}", motorcycleId);

            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting motorcycle");
            return Result.Failure(Error.Create("MOTORCYCLE_DELETION_FAILED", "Erro ao remover moto"));
        }
    }
}

