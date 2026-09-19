using CetproNicol.Application.DTOs;
using CetproNicol.Application.Exceptions;
using CetproNicol.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CetproNicol.API.Controllers;

/// <summary>
/// CRUD de áreas académicas. Lectura pública; escritura restringida a administradores.
/// </summary>
[ApiController]
[Route("api/areas")]
public class AreasController : ControllerBase
{
    private readonly IAreaService _areaService;

    public AreasController(IAreaService areaService)
    {
        _areaService = areaService;
    }

    /// <summary>Lista todas las áreas (activas e inactivas).</summary>
    [HttpGet]
    [ProducesResponseType(typeof(List<AreaDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var areas = await _areaService.GetAllAsync(cancellationToken);
        return Ok(areas);
    }

    /// <summary>Obtiene un área por su id.</summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(AreaDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var area = await _areaService.GetByIdAsync(id, cancellationToken);
            return Ok(area);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    /// <summary>Crea una nueva área. Requiere rol admin.</summary>
    [HttpPost]
    [Authorize(Roles = "admin")]
    [ProducesResponseType(typeof(AreaDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateAreaDto dto, CancellationToken cancellationToken)
    {
        var area = await _areaService.CreateAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = area.Id }, area);
    }

    /// <summary>Actualiza los datos de un área. Requiere rol admin.</summary>
    [HttpPut("{id:guid}")]
    [Authorize(Roles = "admin")]
    [ProducesResponseType(typeof(AreaDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateAreaDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var area = await _areaService.UpdateAsync(id, dto, cancellationToken);
            return Ok(area);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    /// <summary>Elimina lógicamente un área (Activo = false). Requiere rol admin.</summary>
    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            await _areaService.DeleteAsync(id, cancellationToken);
            return NoContent();
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}
