using CetproNicol.Application.DTOs;
using CetproNicol.Application.Exceptions;
using CetproNicol.Application.Interfaces;
using CetproNicol.Domain.Entities;

namespace CetproNicol.Application.Services;

public class AreaService : IAreaService
{
    private readonly IAreaRepository _areaRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AreaService(IAreaRepository areaRepository, IUnitOfWork unitOfWork)
    {
        _areaRepository = areaRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<AreaDto> CreateAsync(CreateAreaDto dto, CancellationToken cancellationToken = default)
    {
        var area = new Area
        {
            Id = Guid.NewGuid(),
            Nombre = dto.Nombre,
            Descripcion = dto.Descripcion,
            ImagenUrl = dto.ImagenUrl,
            Activo = true,
            FechaCreacion = DateTime.UtcNow
        };

        await _areaRepository.AddAsync(area, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return MapToDto(area);
    }

    public async Task<List<AreaDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var areas = await _areaRepository.GetAllAsync(cancellationToken);
        return areas.Select(MapToDto).ToList();
    }

    public async Task<AreaDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var area = await _areaRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException("El área no existe.");

        return MapToDto(area);
    }

    public async Task<AreaDto> UpdateAsync(Guid id, UpdateAreaDto dto, CancellationToken cancellationToken = default)
    {
        var area = await _areaRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException("El área no existe.");

        area.Nombre = dto.Nombre;
        area.Descripcion = dto.Descripcion;
        area.ImagenUrl = dto.ImagenUrl;

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return MapToDto(area);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var area = await _areaRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException("El área no existe.");

        area.Activo = false;

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    private static AreaDto MapToDto(Area area) => new()
    {
        Id = area.Id,
        Nombre = area.Nombre,
        Descripcion = area.Descripcion,
        ImagenUrl = area.ImagenUrl,
        Activo = area.Activo,
        FechaCreacion = area.FechaCreacion
    };
}
