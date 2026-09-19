using CetproNicol.Application.DTOs;

namespace CetproNicol.Application.Interfaces;

public interface IPlanEstudioService
{
    Task<PlanEstudioDto> CreateAsync(CreatePlanEstudioDto dto, CancellationToken cancellationToken = default);

    Task<List<PlanEstudioDto>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<List<PlanEstudioDto>> GetByCursoIdAsync(Guid cursoId, CancellationToken cancellationToken = default);

    Task<PlanEstudioDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<PlanEstudioDto> UpdateAsync(Guid id, UpdatePlanEstudioDto dto, CancellationToken cancellationToken = default);

    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
