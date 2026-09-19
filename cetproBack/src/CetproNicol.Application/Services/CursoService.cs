using CetproNicol.Application.DTOs;
using CetproNicol.Application.Exceptions;
using CetproNicol.Application.Interfaces;
using CetproNicol.Domain.Entities;

namespace CetproNicol.Application.Services;

public class CursoService : ICursoService
{
    private readonly ICursoRepository _cursoRepository;
    private readonly IAreaRepository _areaRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CursoService(ICursoRepository cursoRepository, IAreaRepository areaRepository, IUnitOfWork unitOfWork)
    {
        _cursoRepository = cursoRepository;
        _areaRepository = areaRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<CursoDto> CreateAsync(CreateCursoDto dto, CancellationToken cancellationToken = default)
    {
        _ = await _areaRepository.GetByIdAsync(dto.AreaId, cancellationToken)
            ?? throw new NotFoundException("El área indicada no existe.");

        var curso = new Curso
        {
            Id = Guid.NewGuid(),
            AreaId = dto.AreaId,
            Nombre = dto.Nombre,
            Descripcion = dto.Descripcion,
            ImagenUrl = dto.ImagenUrl,
            Activo = true,
            FechaCreacion = DateTime.UtcNow
        };

        await _cursoRepository.AddAsync(curso, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return MapToDto(curso);
    }

    public async Task<List<CursoDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var cursos = await _cursoRepository.GetAllAsync(cancellationToken);
        return cursos.Select(MapToDto).ToList();
    }

    public async Task<List<CursoDto>> GetByAreaIdAsync(Guid areaId, CancellationToken cancellationToken = default)
    {
        var cursos = await _cursoRepository.GetByAreaIdAsync(areaId, cancellationToken);
        return cursos.Select(MapToDto).ToList();
    }

    public async Task<CursoDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var curso = await _cursoRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException("El curso no existe.");

        return MapToDto(curso);
    }

    public async Task<CursoDto> UpdateAsync(Guid id, UpdateCursoDto dto, CancellationToken cancellationToken = default)
    {
        var curso = await _cursoRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException("El curso no existe.");

        _ = await _areaRepository.GetByIdAsync(dto.AreaId, cancellationToken)
            ?? throw new NotFoundException("El área indicada no existe.");

        curso.AreaId = dto.AreaId;
        curso.Nombre = dto.Nombre;
        curso.Descripcion = dto.Descripcion;
        curso.ImagenUrl = dto.ImagenUrl;

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return MapToDto(curso);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var curso = await _cursoRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException("El curso no existe.");

        curso.Activo = false;

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    private static CursoDto MapToDto(Curso curso) => new()
    {
        Id = curso.Id,
        AreaId = curso.AreaId,
        Nombre = curso.Nombre,
        Descripcion = curso.Descripcion,
        ImagenUrl = curso.ImagenUrl,
        Activo = curso.Activo,
        FechaCreacion = curso.FechaCreacion
    };
}
