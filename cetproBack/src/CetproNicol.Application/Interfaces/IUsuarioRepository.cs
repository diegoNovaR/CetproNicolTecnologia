using CetproNicol.Domain.Entities;

namespace CetproNicol.Application.Interfaces;

public interface IUsuarioRepository
{
    Task<Usuario?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}
