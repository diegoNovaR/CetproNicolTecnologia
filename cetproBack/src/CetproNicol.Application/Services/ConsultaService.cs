using CetproNicol.Application.DTOs;
using CetproNicol.Application.Exceptions;
using CetproNicol.Application.Interfaces;
using CetproNicol.Domain.Entities;
using CetproNicol.Domain.Enums;

namespace CetproNicol.Application.Services;

public class ConsultaService : IConsultaService
{
    private readonly IConsultaRepository _consultaRepository;
    private readonly IAreaRepository _areaRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ConsultaService(IConsultaRepository consultaRepository, IAreaRepository areaRepository, IUnitOfWork unitOfWork)
    {
        _consultaRepository = consultaRepository;
        _areaRepository = areaRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ConsultaDto> CrearAsync(CreateConsultaDto dto, CancellationToken cancellationToken = default)
    {
        if (dto.AreaId.HasValue)
        {
            _ = await _areaRepository.GetByIdAsync(dto.AreaId.Value, cancellationToken)
                ?? throw new NotFoundException("El área indicada no existe.");
        }

        var consulta = new Consulta
        {
            Id = Guid.NewGuid(),
            NombreContacto = dto.NombreContacto,
            Telefono = dto.Telefono,
            Email = dto.Email,
            AreaId = dto.AreaId,
            Mensaje = dto.Mensaje,
            Estado = EstadoConsulta.Nueva,
            Origen = dto.Origen,
            FechaCreacion = DateTime.UtcNow
        };

        await _consultaRepository.AddAsync(consulta, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return MapToDto(consulta);
    }

    private static string MapEstado(EstadoConsulta estado) => estado switch
    {
        EstadoConsulta.Nueva => "nueva",
        EstadoConsulta.Contactada => "contactada",
        EstadoConsulta.Matriculada => "matriculada",
        _ => throw new ArgumentOutOfRangeException(nameof(estado))
    };

    private static ConsultaDto MapToDto(Consulta consulta) => new()
    {
        Id = consulta.Id,
        NombreContacto = consulta.NombreContacto,
        Telefono = consulta.Telefono,
        Email = consulta.Email,
        AreaId = consulta.AreaId,
        Mensaje = consulta.Mensaje,
        Estado = MapEstado(consulta.Estado),
        Origen = consulta.Origen,
        FechaCreacion = consulta.FechaCreacion
    };
}
