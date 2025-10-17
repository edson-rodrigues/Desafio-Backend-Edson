using Mottu.Domain.Entities;

namespace Mottu.Domain.Interfaces;

public interface IDeliveryDriverRepository
{
    Task<DeliveryDriver?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<DeliveryDriver?> GetByCNPJAsync(string cnpj, CancellationToken cancellationToken = default);
    Task<DeliveryDriver?> GetByCNHNumberAsync(string cnhNumber, CancellationToken cancellationToken = default);
    Task<IEnumerable<DeliveryDriver>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(DeliveryDriver driver, CancellationToken cancellationToken = default);
    Task UpdateAsync(DeliveryDriver driver, CancellationToken cancellationToken = default);
    Task<bool> ExistsByCNPJAsync(string cnpj, CancellationToken cancellationToken = default);
    Task<bool> ExistsByCNHNumberAsync(string cnhNumber, CancellationToken cancellationToken = default);
}

