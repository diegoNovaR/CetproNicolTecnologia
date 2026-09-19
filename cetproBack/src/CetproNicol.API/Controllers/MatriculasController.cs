using CetproNicol.Application.DTOs;
using CetproNicol.Application.Exceptions;
using CetproNicol.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CetproNicol.API.Controllers;

/// <summary>
/// Matriculación de estudiantes, aprobación de pagos y consulta de deuda.
/// </summary>
[ApiController]
public class MatriculasController : ControllerBase
{
    private readonly IMatriculaService _matriculaService;

    public MatriculasController(IMatriculaService matriculaService)
    {
        _matriculaService = matriculaService;
    }

    /// <summary>
    /// Matricula a un estudiante en un plan de estudio: crea la matrícula en estado "pendiente"
    /// y genera el pago de matrícula más una cuota por cada mes de duración del plan, todo en una transacción.
    /// </summary>
    /// <param name="request">Id del usuario (debe tener rol estudiante) y del plan de estudio.</param>
    /// <response code="201">Matrícula creada junto con sus pagos.</response>
    /// <response code="400">El usuario no es estudiante, el plan no está activo, o ya tiene una matrícula pendiente/activa para ese curso.</response>
    /// <response code="404">El usuario o el plan de estudio no existen.</response>
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

    /// <summary>
    /// Aprueba un pago pendiente. Si es el primer pago aprobado de la matrícula, esta pasa de
    /// "pendiente" a "activa" y se registra su fecha de aprobación.
    /// </summary>
    /// <param name="id">Id del pago a aprobar.</param>
    /// <response code="200">Pago aprobado.</response>
    /// <response code="400">El pago no está en estado "pendiente".</response>
    /// <response code="404">El pago no existe.</response>
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

    /// <summary>Calcula la deuda pendiente de una matrícula: suma de cuotas no pagadas y su detalle por periodo.</summary>
    /// <param name="id">Id de la matrícula.</param>
    /// <response code="200">Resumen de deuda calculado.</response>
    /// <response code="404">La matrícula no existe.</response>
    [HttpGet("/api/matriculas/{id:guid}/deuda")]
    [ProducesResponseType(typeof(DeudaEstudianteDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CalcularDeuda(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var deuda = await _matriculaService.CalcularDeudaEstudianteAsync(id, cancellationToken);
            return Ok(deuda);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}
