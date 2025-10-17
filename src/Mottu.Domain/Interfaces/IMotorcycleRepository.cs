using Mottu.Domain.Entities;

namespace Mottu.Domain.Interfaces;

public interface IMotorcycleRepository
{
    Task<Motorcycle?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Motorcycle?> GetByLicensePlateAsync(string licensePlate, CancellationToken cancellationToken = default);
    Task<IEnumerable<Motorcycle>> GetAllAsync(string? licensePlateFilter = null, CancellationToken cancellationToken = default);
    Task AddAsync(Motorcycle motorcycle, CancellationToken cancellationToken = default);
    Task UpdateAsync(Motorcycle motorcycle, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> HasRentalsAsync(Guid motorcycleId, CancellationToken cancellationToken = default);
    Task<bool> ExistsByLicensePlateAsync(string licensePlate, CancellationToken cancellationToken = default);
}

