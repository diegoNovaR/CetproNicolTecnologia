using CetproNicol.Domain.Entities;

namespace CetproNicol.Application.Interfaces;

public interface IMatriculaRepository
{
    Task<List<Matricula>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<List<Matricula>> GetAllConDetalleAsync(Guid? usuarioId, CancellationToken cancellationToken = default);

    Task<Matricula?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<bool> ExisteMatriculaActivaParaCursoAsync(Guid usuarioId, Guid cursoId, CancellationToken cancellationToken = default);

    Task<bool> ExisteMatriculaParaCursoAsync(Guid usuarioId, Guid cursoId, CancellationToken cancellationToken = default);

    Task<Matricula?> GetActivaConPagosPorCursoAsync(Guid usuarioId, Guid cursoId, CancellationToken cancellationToken = default);

    Task AddAsync(Matricula matricula, CancellationToken cancellationToken = default);
}
