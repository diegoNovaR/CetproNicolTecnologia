using CetproNicol.Application.DTOs;

namespace CetproNicol.Application.Interfaces;

public interface IContenidoService
{
    Task<VerificarAccesoDto> VerificarAccesoContenidoAsync(Guid usuarioId, Guid cursoId, CancellationToken cancellationToken = default);
}
