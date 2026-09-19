using CetproNicol.Application.DTOs;

namespace CetproNicol.Application.Interfaces;

public interface IAreaService
{
    Task<AreaDto> CreateAsync(CreateAreaDto dto, CancellationToken cancellationToken = default);

    Task<List<AreaDto>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<AreaDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<AreaDto> UpdateAsync(Guid id, UpdateAreaDto dto, CancellationToken cancellationToken = default);

    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
