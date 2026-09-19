using CetproNicol.Domain.Entities;

namespace CetproNicol.Application.Interfaces;

public interface IUsuarioRepository
{
    Task<List<Usuario>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<Usuario?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<Usuario?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);

    Task AddAsync(Usuario usuario, CancellationToken cancellationToken = default);
}
