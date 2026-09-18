using CetproNicol.Application.Interfaces;
using CetproNicol.Domain.Entities;
using CetproNicol.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CetproNicol.Infrastructure.Repositories;

public class PlanEstudioRepository : IPlanEstudioRepository
{
    private readonly CetproNicolDbContext _context;

    public PlanEstudioRepository(CetproNicolDbContext context)
    {
        _context = context;
    }

    public Task<PlanEstudio?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        _context.PlanesEstudio
            .Include(p => p.Curso)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
}
