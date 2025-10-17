using Microsoft.EntityFrameworkCore;
using Mottu.Domain.Entities;
using Mottu.Domain.Enums;
using Mottu.Domain.Interfaces;

namespace Mottu.Infrastructure.Persistence.PostgreSQL.Repositories;

public class RentalRepository : IRentalRepository
{
    private readonly ApplicationDbContext _context;

    public RentalRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Rental?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Rentals
            .Include(r => r.Motorcycle)
            .Include(r => r.DeliveryDriver)
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Rental>> GetByDeliveryDriverIdAsync(Guid driverId, CancellationToken cancellationToken = default)
    {
        return await _context.Rentals
            .Include(r => r.Motorcycle)
            .Include(r => r.DeliveryDriver)
            .Where(r => r.DeliveryDriverId == driverId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Rental>> GetByMotorcycleIdAsync(Guid motorcycleId, CancellationToken cancellationToken = default)
    {
        return await _context.Rentals
            .Include(r => r.Motorcycle)
            .Include(r => r.DeliveryDriver)
            .Where(r => r.MotorcycleId == motorcycleId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Rental>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Rentals
            .Include(r => r.Motorcycle)
            .Include(r => r.DeliveryDriver)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Rental rental, CancellationToken cancellationToken = default)
    {
        await _context.Rentals.AddAsync(rental, cancellationToken);
    }

    public Task UpdateAsync(Rental rental, CancellationToken cancellationToken = default)
    {
        _context.Rentals.Update(rental);
        return Task.CompletedTask;
    }

    public async Task<bool> HasActiveRentalAsync(Guid driverId, CancellationToken cancellationToken = default)
    {
        return await _context.Rentals
            .AnyAsync(r => r.DeliveryDriverId == driverId && r.Status == RentalStatus.Active, cancellationToken);
    }
}

