using CetproNicol.Application.DTOs;

namespace CetproNicol.Application.Interfaces;

public interface IDashboardService
{
    Task<DashboardDto> GetResumenAsync(CancellationToken cancellationToken = default);
}
