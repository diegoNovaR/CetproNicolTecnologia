using CetproNicol.Domain.Entities;

namespace CetproNicol.Application.Interfaces;

public interface IConsultaRepository
{
    Task<List<Consulta>> GetAllAsync(CancellationToken cancellationToken = default);

    Task AddAsync(Consulta consulta, CancellationToken cancellationToken = default);
}
