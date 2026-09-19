using CetproNicol.Domain.Entities;

namespace CetproNicol.Application.Interfaces;

public interface IAreaRepository
{
    Task<List<Area>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<Area?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task AddAsync(Area area, CancellationToken cancellationToken = default);
}
