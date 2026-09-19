using CetproNicol.Domain.Entities;

namespace CetproNicol.Application.Interfaces;

public interface IPlanEstudioRepository
{
    Task<List<PlanEstudio>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<List<PlanEstudio>> GetByCursoIdAsync(Guid cursoId, CancellationToken cancellationToken = default);

    Task<PlanEstudio?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task AddAsync(PlanEstudio planEstudio, CancellationToken cancellationToken = default);
}
