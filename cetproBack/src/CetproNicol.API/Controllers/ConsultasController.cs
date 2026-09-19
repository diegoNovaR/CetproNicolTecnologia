using CetproNicol.Application.DTOs;
using CetproNicol.Application.Exceptions;
using CetproNicol.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CetproNicol.API.Controllers;

/// <summary>
/// Endpoints públicos para que visitantes del sitio dejen consultas de contacto.
/// </summary>
[ApiController]
[Route("api/consultas")]
public class ConsultasController : ControllerBase
{
    private readonly IConsultaService _consultaService;

    public ConsultasController(IConsultaService consultaService)
    {
        _consultaService = consultaService;
    }

    /// <summary>
    /// Registra una consulta de contacto enviada desde el sitio público (formulario web, landing, etc).
    /// </summary>
    /// <param name="dto">Datos de contacto y mensaje del interesado.</param>
    /// <response code="201">La consulta fue registrada con estado "nueva".</response>
    /// <response code="400">Datos de entrada inválidos.</response>
    /// <response code="404">El área indicada (si se envía) no existe.</response>
    [HttpPost]
    [ProducesResponseType(typeof(ConsultaDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Crear([FromBody] CreateConsultaDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var consulta = await _consultaService.CrearAsync(dto, cancellationToken);
            return StatusCode(StatusCodes.Status201Created, consulta);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}
