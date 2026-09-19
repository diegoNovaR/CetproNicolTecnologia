using CetproNicol.Application.DTOs;
using CetproNicol.Application.Interfaces;
using CetproNicol.Domain.Enums;

namespace CetproNicol.Application.Services;

public class ContenidoService : IContenidoService
{
    private readonly IMatriculaRepository _matriculaRepository;

    public ContenidoService(IMatriculaRepository matriculaRepository)
    {
        _matriculaRepository = matriculaRepository;
    }

    public async Task<VerificarAccesoDto> VerificarAccesoContenidoAsync(Guid usuarioId, Guid cursoId, CancellationToken cancellationToken = default)
    {
        var matriculaActiva = await _matriculaRepository.GetActivaConPagosPorCursoAsync(usuarioId, cursoId, cancellationToken);

        if (matriculaActiva is null)
        {
            var existeAlgunaMatricula = await _matriculaRepository.ExisteMatriculaParaCursoAsync(usuarioId, cursoId, cancellationToken);

            return new VerificarAccesoDto
            {
                TieneAcceso = false,
                MotivoRechazo = existeAlgunaMatricula ? "matrícula inactiva" : "sin matrícula"
            };
        }

        var periodoActual = DateTime.UtcNow.ToString("yyyy-MM");
        var tienePagosAtrasados = matriculaActiva.Pagos.Any(p =>
            string.Compare(p.Periodo, periodoActual, StringComparison.Ordinal) <= 0
            && p.Estado != EstadoPago.Aprobado);

        if (tienePagosAtrasados)
        {
            return new VerificarAccesoDto
            {
                TieneAcceso = false,
                MotivoRechazo = "pagos atrasados"
            };
        }

        return new VerificarAccesoDto
        {
            TieneAcceso = true,
            MotivoRechazo = null
        };
    }
}
