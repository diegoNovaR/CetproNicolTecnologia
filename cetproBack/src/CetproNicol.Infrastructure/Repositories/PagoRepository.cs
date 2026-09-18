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

    public Task<Pago?> GetByIdWithMatriculaAsync(Guid id, CancellationToken cancellationToken = default) =>
        _context.Pagos
            .Include(p => p.Matricula)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

    public async Task AddRangeAsync(IEnumerable<Pago> pagos, CancellationToken cancellationToken = default) =>
        await _context.Pagos.AddRangeAsync(pagos, cancellationToken);

    public Task<bool> ExisteAlgunPagoAprobadoAsync(Guid matriculaId, CancellationToken cancellationToken = default) =>
        _context.Pagos.AnyAsync(p => p.MatriculaId == matriculaId && p.Estado == EstadoPago.Aprobado, cancellationToken);
}
