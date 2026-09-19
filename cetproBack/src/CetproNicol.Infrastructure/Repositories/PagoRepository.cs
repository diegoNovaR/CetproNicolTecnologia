using CetproNicol.Application.Interfaces;
using CetproNicol.Domain.Entities;
using CetproNicol.Domain.Enums;
using CetproNicol.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CetproNicol.Infrastructure.Repositories;

public class PagoRepository : IPagoRepository
{
    private readonly CetproNicolDbContext _context;

    public PagoRepository(CetproNicolDbContext context)
    {
        _context = context;
    }

    public Task<List<Pago>> GetAllAsync(CancellationToken cancellationToken = default) =>
        _context.Pagos.ToListAsync(cancellationToken);

    public Task<Pago?> GetByIdWithMatriculaAsync(Guid id, CancellationToken cancellationToken = default) =>
        _context.Pagos
            .Include(p => p.Matricula)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

    public Task<List<Pago>> GetAllConDetalleAsync(EstadoPago? estado, CancellationToken cancellationToken = default)
    {
        var query = _context.Pagos
            .Include(p => p.Matricula)
                .ThenInclude(m => m.Usuario)
            .Include(p => p.Matricula)
                .ThenInclude(m => m.PlanEstudio)
                    .ThenInclude(p => p.Curso)
            .AsQueryable();

        if (estado.HasValue)
            query = query.Where(p => p.Estado == estado.Value);

        return query.OrderBy(p => p.Periodo).ThenBy(p => p.NumeroCuota).ToListAsync(cancellationToken);
    }

    public Task<List<Pago>> GetPendientesByMatriculaIdAsync(Guid matriculaId, CancellationToken cancellationToken = default) =>
        _context.Pagos
            .Where(p => p.MatriculaId == matriculaId && p.Estado == EstadoPago.Pendiente)
            .OrderBy(p => p.NumeroCuota)
            .ToListAsync(cancellationToken);

    public async Task AddRangeAsync(IEnumerable<Pago> pagos, CancellationToken cancellationToken = default) =>
        await _context.Pagos.AddRangeAsync(pagos, cancellationToken);

    public Task<bool> ExisteAlgunPagoAprobadoAsync(Guid matriculaId, CancellationToken cancellationToken = default) =>
        _context.Pagos.AnyAsync(p => p.MatriculaId == matriculaId && p.Estado == EstadoPago.Aprobado, cancellationToken);
}
