using Microsoft.EntityFrameworkCore;
using Mottu.Domain.Entities;
using Mottu.Domain.Interfaces;

namespace Mottu.Infrastructure.Persistence.PostgreSQL.Repositories;

public class DeliveryDriverRepository : IDeliveryDriverRepository
{
    private readonly ApplicationDbContext _context;

    public DeliveryDriverRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<DeliveryDriver?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.DeliveryDrivers
            .FirstOrDefaultAsync(d => d.Id == id, cancellationToken);
    }

    public async Task<DeliveryDriver?> GetByCNPJAsync(string cnpj, CancellationToken cancellationToken = default)
    {
        return await _context.DeliveryDrivers
            .FirstOrDefaultAsync(d => d.CNPJ == cnpj, cancellationToken);
    }

    public async Task<DeliveryDriver?> GetByCNHNumberAsync(string cnhNumber, CancellationToken cancellationToken = default)
    {
        return await _context.DeliveryDrivers
            .FirstOrDefaultAsync(d => d.CNH.Number == cnhNumber, cancellationToken);
    }

    public async Task<IEnumerable<DeliveryDriver>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.DeliveryDrivers.ToListAsync(cancellationToken);
    }

    public async Task AddAsync(DeliveryDriver driver, CancellationToken cancellationToken = default)
    {
        await _context.DeliveryDrivers.AddAsync(driver, cancellationToken);
    }

    public Task UpdateAsync(DeliveryDriver driver, CancellationToken cancellationToken = default)
    {
        _context.DeliveryDrivers.Update(driver);
        return Task.CompletedTask;
    }

    public async Task<bool> ExistsByCNPJAsync(string cnpj, CancellationToken cancellationToken = default)
    {
        return await _context.DeliveryDrivers
            .AnyAsync(d => d.CNPJ == cnpj, cancellationToken);
    }

    public async Task<bool> ExistsByCNHNumberAsync(string cnhNumber, CancellationToken cancellationToken = default)
    {
        return await _context.DeliveryDrivers
            .AnyAsync(d => d.CNH.Number == cnhNumber, cancellationToken);
    }
}

