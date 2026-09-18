using CetproNicol.Application.Interfaces;
using CetproNicol.Domain.Entities;
using CetproNicol.Domain.Enums;
using CetproNicol.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CetproNicol.Infrastructure.Repositories;

public class MatriculaRepository : IMatriculaRepository
{
    private readonly CetproNicolDbContext _context;

    public MatriculaRepository(CetproNicolDbContext context)
    {
        _context = context;
    }

    public Task<Matricula?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        _context.Matriculas.FirstOrDefaultAsync(m => m.Id == id, cancellationToken);

    public Task<bool> ExisteMatriculaActivaParaCursoAsync(Guid usuarioId, Guid cursoId, CancellationToken cancellationToken = default) =>
        _context.Matriculas
            .Include(m => m.PlanEstudio)
            .AnyAsync(m => m.UsuarioId == usuarioId
                && m.PlanEstudio.CursoId == cursoId
                && (m.Estado == EstadoMatricula.Pendiente || m.Estado == EstadoMatricula.Activa),
                cancellationToken);

    public async Task AddAsync(Matricula matricula, CancellationToken cancellationToken = default) =>
        await _context.Matriculas.AddAsync(matricula, cancellationToken);
}
