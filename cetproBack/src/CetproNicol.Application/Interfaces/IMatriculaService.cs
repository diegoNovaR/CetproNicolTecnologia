using CetproNicol.Application.DTOs;

namespace CetproNicol.Application.Interfaces;

public interface IMatriculaService
{
    Task<MatriculaDto> MatricularEstudianteAsync(Guid usuarioId, Guid planEstudioId, CancellationToken cancellationToken = default);

    Task<PagoDto> AprobarPagoAsync(Guid pagoId, CancellationToken cancellationToken = default);

    Task<DeudaEstudianteDto> CalcularDeudaEstudianteAsync(Guid matriculaId, CancellationToken cancellationToken = default);
}
