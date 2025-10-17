using Mottu.Domain.Entities;

namespace Mottu.Domain.Interfaces;

public interface IRentalRepository
{
    Task<Rental?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Rental>> GetByDeliveryDriverIdAsync(Guid driverId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Rental>> GetByMotorcycleIdAsync(Guid motorcycleId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Rental>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(Rental rental, CancellationToken cancellationToken = default);
    Task UpdateAsync(Rental rental, CancellationToken cancellationToken = default);
    Task<bool> HasActiveRentalAsync(Guid driverId, CancellationToken cancellationToken = default);
}

