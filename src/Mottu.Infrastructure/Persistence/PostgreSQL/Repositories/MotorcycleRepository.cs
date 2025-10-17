using Microsoft.EntityFrameworkCore;
using Mottu.Domain.Entities;
using Mottu.Domain.Interfaces;

namespace Mottu.Infrastructure.Persistence.PostgreSQL.Repositories;

public class MotorcycleRepository : IMotorcycleRepository
{
    private readonly ApplicationDbContext _context;

    public MotorcycleRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Motorcycle?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Motorcycles
            .FirstOrDefaultAsync(m => m.Id == id, cancellationToken);
    }

    public async Task<Motorcycle?> GetByLicensePlateAsync(string licensePlate, CancellationToken cancellationToken = default)
    {
        return await _context.Motorcycles
            .FirstOrDefaultAsync(m => m.LicensePlate == licensePlate, cancellationToken);
    }

    public async Task<IEnumerable<Motorcycle>> GetAllAsync(string? licensePlateFilter = null, CancellationToken cancellationToken = default)
    {
        var query = _context.Motorcycles.AsQueryable();

        if (!string.IsNullOrWhiteSpace(licensePlateFilter))
        {
            query = query.Where(m => m.LicensePlate == licensePlateFilter);
        }

        return await query.ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Motorcycle motorcycle, CancellationToken cancellationToken = default)
    {
        await _context.Motorcycles.AddAsync(motorcycle, cancellationToken);
    }

    public Task UpdateAsync(Motorcycle motorcycle, CancellationToken cancellationToken = default)
    {
        _context.Motorcycles.Update(motorcycle);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var motorcycle = _context.Motorcycles.Find(id);
        if (motorcycle != null)
        {
            _context.Motorcycles.Remove(motorcycle);
        }
        return Task.CompletedTask;
    }

    public async Task<bool> HasRentalsAsync(Guid motorcycleId, CancellationToken cancellationToken = default)
    {
        return await _context.Rentals
            .AnyAsync(r => r.MotorcycleId == motorcycleId, cancellationToken);
    }

    public async Task<bool> ExistsByLicensePlateAsync(string licensePlate, CancellationToken cancellationToken = default)
    {
        return await _context.Motorcycles
            .AnyAsync(m => m.LicensePlate == licensePlate, cancellationToken);
    }
}

