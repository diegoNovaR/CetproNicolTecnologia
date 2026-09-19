using CetproNicol.Domain.Entities;

namespace CetproNicol.Application.Interfaces;

public interface ICursoRepository
{
    Task<List<Curso>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<List<Curso>> GetByAreaIdAsync(Guid areaId, CancellationToken cancellationToken = default);

    Task<Curso?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task AddAsync(Curso curso, CancellationToken cancellationToken = default);
}
