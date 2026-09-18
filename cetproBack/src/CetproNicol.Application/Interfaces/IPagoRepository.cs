using CetproNicol.Domain.Entities;

namespace CetproNicol.Application.Interfaces;

public interface IPagoRepository
{
    Task<Pago?> GetByIdWithMatriculaAsync(Guid id, CancellationToken cancellationToken = default);

    Task AddRangeAsync(IEnumerable<Pago> pagos, CancellationToken cancellationToken = default);

    Task<bool> ExisteAlgunPagoAprobadoAsync(Guid matriculaId, CancellationToken cancellationToken = default);
}
