using CetproNicol.Application.DTOs;
using CetproNicol.Application.Interfaces;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CetproNicol.API.Controllers;

/// <summary>
/// Verificación de acceso a contenido de curso según el estado de matrícula y pagos del estudiante.
/// </summary>
[ApiController]
[Authorize]
public class ContenidosController : ControllerBase
{
    private readonly IContenidoService _contenidoService;

    public ContenidosController(IContenidoService contenidoService)
    {
        _contenidoService = contenidoService;
    }

    /// <summary>
    /// Verifica si un usuario tiene acceso al contenido de un curso: requiere una matrícula activa
    /// y los pagos al día hasta el mes en curso.
    /// </summary>
    /// <param name="usuarioId">Id del usuario.</param>
    /// <param name="cursoId">Id del curso.</param>
    /// <response code="200">
    /// Resultado de la verificación. Si <c>tieneAcceso</c> es false, <c>motivoRechazo</c> indica la causa
    /// ("sin matrícula", "matrícula inactiva" o "pagos atrasados").
    /// </response>
    [HttpGet("/api/contenidos/verificar-acceso")]
    [ProducesResponseType(typeof(VerificarAccesoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> VerificarAcceso([FromQuery] Guid usuarioId, [FromQuery] Guid cursoId, CancellationToken cancellationToken)
    {
        // Un estudiante solo puede verificar su propio acceso.
        var actual = Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var id) ? id : (Guid?)null;
        if (!User.IsInRole("admin") && usuarioId != actual) return Forbid();

        var resultado = await _contenidoService.VerificarAccesoContenidoAsync(usuarioId, cursoId, cancellationToken);
        return Ok(resultado);
    }
}
