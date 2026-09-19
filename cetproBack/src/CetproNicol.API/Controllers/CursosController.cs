using CetproNicol.Application.DTOs;
using CetproNicol.Application.Exceptions;
using CetproNicol.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CetproNicol.API.Controllers;

/// <summary>
/// CRUD de cursos, con filtro opcional por área. Lectura pública; escritura restringida a administradores.
/// </summary>
[ApiController]
[Route("api/cursos")]
public class CursosController : ControllerBase
{
    private readonly ICursoService _cursoService;

    public CursosController(ICursoService cursoService)
    {
        _cursoService = cursoService;
    }

    /// <summary>Lista cursos. Si se envía <paramref name="areaId"/>, filtra solo los de esa área.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(List<CursoDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] Guid? areaId, CancellationToken cancellationToken)
    {
        var cursos = areaId.HasValue
            ? await _cursoService.GetByAreaIdAsync(areaId.Value, cancellationToken)
            : await _cursoService.GetAllAsync(cancellationToken);

        return Ok(cursos);
    }

    /// <summary>Obtiene un curso por su id.</summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(CursoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var curso = await _cursoService.GetByIdAsync(id, cancellationToken);
            return Ok(curso);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    /// <summary>Crea un curso dentro de un área existente. Requiere rol admin.</summary>
    [HttpPost]
    [Authorize(Roles = "admin")]
    [ProducesResponseType(typeof(CursoDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Create([FromBody] CreateCursoDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var curso = await _cursoService.CreateAsync(dto, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = curso.Id }, curso);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    /// <summary>Actualiza los datos de un curso. Requiere rol admin.</summary>
    [HttpPut("{id:guid}")]
    [Authorize(Roles = "admin")]
    [ProducesResponseType(typeof(CursoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCursoDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var curso = await _cursoService.UpdateAsync(id, dto, cancellationToken);
            return Ok(curso);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    /// <summary>Elimina lógicamente un curso (Activo = false). Requiere rol admin.</summary>
    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            await _cursoService.DeleteAsync(id, cancellationToken);
            return NoContent();
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}
