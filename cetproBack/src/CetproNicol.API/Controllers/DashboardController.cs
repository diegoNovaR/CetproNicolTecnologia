using CetproNicol.Application.DTOs;
using CetproNicol.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CetproNicol.API.Controllers;

/// <summary>
/// Panel de indicadores para el administrador.
/// </summary>
[ApiController]
[Route("api/dashboard")]
[Authorize(Roles = "admin")]
public class DashboardController : ControllerBase
{
    private readonly IDashboardService _dashboardService;

    public DashboardController(IDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    /// <summary>
    /// Obtiene el resumen de indicadores del sistema: usuarios, áreas, cursos, matrículas,
    /// pagos (pendientes/aprobados con sus montos) y consultas de contacto.
    /// </summary>
    /// <response code="200">Resumen calculado correctamente.</response>
    /// <response code="401">No se envió un token válido.</response>
    /// <response code="403">El token no corresponde a un usuario con rol admin.</response>
    [HttpGet("admin")]
    [ProducesResponseType(typeof(DashboardDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetResumen(CancellationToken cancellationToken)
    {
        var resumen = await _dashboardService.GetResumenAsync(cancellationToken);
        return Ok(resumen);
    }
}
