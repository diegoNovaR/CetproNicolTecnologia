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

    public Task<List<PlanEstudio>> GetAllAsync(CancellationToken cancellationToken = default) =>
        _context.PlanesEstudio.ToListAsync(cancellationToken);

    public Task<List<PlanEstudio>> GetByCursoIdAsync(Guid cursoId, CancellationToken cancellationToken = default) =>
        _context.PlanesEstudio
            .Where(p => p.CursoId == cursoId)
            .ToListAsync(cancellationToken);

    public Task<PlanEstudio?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        _context.PlanesEstudio
            .Include(p => p.Curso)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

    public async Task AddAsync(PlanEstudio planEstudio, CancellationToken cancellationToken = default) =>
        await _context.PlanesEstudio.AddAsync(planEstudio, cancellationToken);
}
