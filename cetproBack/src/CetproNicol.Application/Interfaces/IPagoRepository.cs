using CetproNicol.Domain.Entities;

namespace CetproNicol.Application.Interfaces;

public interface IPagoRepository
{
    Task<List<Pago>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<Pago?> GetByIdWithMatriculaAsync(Guid id, CancellationToken cancellationToken = default);

    Task<List<Pago>> GetPendientesByMatriculaIdAsync(Guid matriculaId, CancellationToken cancellationToken = default);

    Task AddRangeAsync(IEnumerable<Pago> pagos, CancellationToken cancellationToken = default);

    Task<bool> ExisteAlgunPagoAprobadoAsync(Guid matriculaId, CancellationToken cancellationToken = default);
}
