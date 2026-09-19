using CetproNicol.Application.DTOs;
using CetproNicol.Application.Exceptions;
using CetproNicol.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CetproNicol.API.Controllers;

/// <summary>
/// CRUD de planes de estudio, con filtro opcional por curso. Lectura pública; escritura restringida a administradores.
/// </summary>
[ApiController]
[Route("api/planes-estudio")]
public class PlanesEstudioController : ControllerBase
{
    private readonly IPlanEstudioService _planEstudioService;

    public PlanesEstudioController(IPlanEstudioService planEstudioService)
    {
        _planEstudioService = planEstudioService;
    }

    /// <summary>Lista planes de estudio. Si se envía <paramref name="cursoId"/>, filtra solo los de ese curso.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(List<PlanEstudioDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] Guid? cursoId, CancellationToken cancellationToken)
    {
        var planes = cursoId.HasValue
            ? await _planEstudioService.GetByCursoIdAsync(cursoId.Value, cancellationToken)
            : await _planEstudioService.GetAllAsync(cancellationToken);

        return Ok(planes);
    }

    /// <summary>Obtiene un plan de estudio por su id.</summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(PlanEstudioDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var plan = await _planEstudioService.GetByIdAsync(id, cancellationToken);
            return Ok(plan);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    /// <summary>Crea un plan de estudio ("carrera_completa" o "modulo") para un curso existente. Requiere rol admin.</summary>
    [HttpPost]
    [Authorize(Roles = "admin")]
    [ProducesResponseType(typeof(PlanEstudioDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Create([FromBody] CreatePlanEstudioDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var plan = await _planEstudioService.CreateAsync(dto, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = plan.Id }, plan);
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

    /// <summary>Actualiza los datos de un plan de estudio. Requiere rol admin.</summary>
    [HttpPut("{id:guid}")]
    [Authorize(Roles = "admin")]
    [ProducesResponseType(typeof(PlanEstudioDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdatePlanEstudioDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var plan = await _planEstudioService.UpdateAsync(id, dto, cancellationToken);
            return Ok(plan);
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

    /// <summary>Elimina lógicamente un plan de estudio (Activo = false). Requiere rol admin.</summary>
    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            await _planEstudioService.DeleteAsync(id, cancellationToken);
            return NoContent();
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}
