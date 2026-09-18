using CetproNicol.Application.DTOs;
using CetproNicol.Application.Exceptions;
using CetproNicol.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CetproNicol.API.Controllers;

[ApiController]
public class MatriculasController : ControllerBase
{
    private readonly IMatriculaService _matriculaService;

    public MatriculasController(IMatriculaService matriculaService)
    {
        _matriculaService = matriculaService;
    }

    [HttpPost("/api/matriculas")]
    [ProducesResponseType(typeof(MatriculaDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> MatricularEstudiante([FromBody] MatricularEstudianteRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var matricula = await _matriculaService.MatricularEstudianteAsync(request.UsuarioId, request.PlanEstudioId, cancellationToken);
            return CreatedAtAction(nameof(MatricularEstudiante), new { id = matricula.Id }, matricula);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (BusinessRuleException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("/api/pagos/{id:guid}/aprobar")]
    [ProducesResponseType(typeof(PagoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AprobarPago(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var pago = await _matriculaService.AprobarPagoAsync(id, cancellationToken);
            return Ok(pago);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (BusinessRuleException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
