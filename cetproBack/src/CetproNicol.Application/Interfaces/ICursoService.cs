using CetproNicol.Application.DTOs;

namespace CetproNicol.Application.Interfaces;

public interface ICursoService
{
    Task<CursoDto> CreateAsync(CreateCursoDto dto, CancellationToken cancellationToken = default);

    Task<List<CursoDto>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<List<CursoDto>> GetByAreaIdAsync(Guid areaId, CancellationToken cancellationToken = default);

    Task<CursoDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<CursoDto> UpdateAsync(Guid id, UpdateCursoDto dto, CancellationToken cancellationToken = default);

    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
