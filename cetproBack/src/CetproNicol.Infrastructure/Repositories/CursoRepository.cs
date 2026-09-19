using CetproNicol.Application.Interfaces;
using CetproNicol.Domain.Entities;
using CetproNicol.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CetproNicol.Infrastructure.Repositories;

public class CursoRepository : ICursoRepository
{
    private readonly CetproNicolDbContext _context;

    public CursoRepository(CetproNicolDbContext context)
    {
        _context = context;
    }

    public Task<List<Curso>> GetAllAsync(CancellationToken cancellationToken = default) =>
        _context.Cursos.OrderBy(c => c.Nombre).ToListAsync(cancellationToken);

    public Task<List<Curso>> GetByAreaIdAsync(Guid areaId, CancellationToken cancellationToken = default) =>
        _context.Cursos
            .Where(c => c.AreaId == areaId)
            .OrderBy(c => c.Nombre)
            .ToListAsync(cancellationToken);

    public Task<Curso?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        _context.Cursos.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

    public async Task AddAsync(Curso curso, CancellationToken cancellationToken = default) =>
        await _context.Cursos.AddAsync(curso, cancellationToken);
}
