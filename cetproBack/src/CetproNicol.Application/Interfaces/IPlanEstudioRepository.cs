using CetproNicol.Domain.Entities;

namespace CetproNicol.Application.Interfaces;

public interface IPlanEstudioRepository
{
    Task<PlanEstudio?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}
