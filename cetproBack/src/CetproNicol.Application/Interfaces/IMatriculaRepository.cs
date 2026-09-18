using CetproNicol.Domain.Entities;

namespace CetproNicol.Application.Interfaces;

public interface IMatriculaRepository
{
    Task<Matricula?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<bool> ExisteMatriculaActivaParaCursoAsync(Guid usuarioId, Guid cursoId, CancellationToken cancellationToken = default);

    Task AddAsync(Matricula matricula, CancellationToken cancellationToken = default);
}
