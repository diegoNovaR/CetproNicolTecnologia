using CetproNicol.Application.Interfaces;
using CetproNicol.Domain.Entities;
using CetproNicol.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CetproNicol.Infrastructure.Repositories;

public class AreaRepository : IAreaRepository
{
    private readonly CetproNicolDbContext _context;

    public AreaRepository(CetproNicolDbContext context)
    {
        _context = context;
    }

    public Task<List<Area>> GetAllAsync(CancellationToken cancellationToken = default) =>
        _context.Areas.OrderBy(a => a.Nombre).ToListAsync(cancellationToken);

    public Task<Area?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        _context.Areas.FirstOrDefaultAsync(a => a.Id == id, cancellationToken);

    public async Task AddAsync(Area area, CancellationToken cancellationToken = default) =>
        await _context.Areas.AddAsync(area, cancellationToken);
}
