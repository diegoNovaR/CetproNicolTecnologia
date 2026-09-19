using CetproNicol.Application.DTOs;
using CetproNicol.Application.Exceptions;
using CetproNicol.Application.Interfaces;
using CetproNicol.Domain.Entities;
using CetproNicol.Domain.Enums;

namespace CetproNicol.Application.Services;

public class PlanEstudioService : IPlanEstudioService
{
    private readonly IPlanEstudioRepository _planEstudioRepository;
    private readonly ICursoRepository _cursoRepository;
    private readonly IUnitOfWork _unitOfWork;

    public PlanEstudioService(IPlanEstudioRepository planEstudioRepository, ICursoRepository cursoRepository, IUnitOfWork unitOfWork)
    {
        _planEstudioRepository = planEstudioRepository;
        _cursoRepository = cursoRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<PlanEstudioDto> CreateAsync(CreatePlanEstudioDto dto, CancellationToken cancellationToken = default)
    {
        _ = await _cursoRepository.GetByIdAsync(dto.CursoId, cancellationToken)
            ?? throw new NotFoundException("El curso indicado no existe.");

        var planEstudio = new PlanEstudio
        {
            Id = Guid.NewGuid(),
            CursoId = dto.CursoId,
            Tipo = ParseTipo(dto.Tipo),
            DuracionMeses = dto.DuracionMeses,
            PrecioMatricula = dto.PrecioMatricula,
            PensionMensual = dto.PensionMensual,
            Activo = true
        };

        await _planEstudioRepository.AddAsync(planEstudio, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return MapToDto(planEstudio);
    }

    public async Task<List<PlanEstudioDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var planes = await _planEstudioRepository.GetAllAsync(cancellationToken);
        return planes.Select(MapToDto).ToList();
    }

    public async Task<List<PlanEstudioDto>> GetByCursoIdAsync(Guid cursoId, CancellationToken cancellationToken = default)
    {
        var planes = await _planEstudioRepository.GetByCursoIdAsync(cursoId, cancellationToken);
        return planes.Select(MapToDto).ToList();
    }

    public async Task<PlanEstudioDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var planEstudio = await _planEstudioRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException("El plan de estudio no existe.");

        return MapToDto(planEstudio);
    }

    public async Task<PlanEstudioDto> UpdateAsync(Guid id, UpdatePlanEstudioDto dto, CancellationToken cancellationToken = default)
    {
        var planEstudio = await _planEstudioRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException("El plan de estudio no existe.");

        planEstudio.Tipo = ParseTipo(dto.Tipo);
        planEstudio.DuracionMeses = dto.DuracionMeses;
        planEstudio.PrecioMatricula = dto.PrecioMatricula;
        planEstudio.PensionMensual = dto.PensionMensual;

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return MapToDto(planEstudio);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var planEstudio = await _planEstudioRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException("El plan de estudio no existe.");

        planEstudio.Activo = false;

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    private static TipoPlanEstudio ParseTipo(string tipo) => tipo switch
    {
        "carrera_completa" => TipoPlanEstudio.CarreraCompleta,
        "modulo" => TipoPlanEstudio.Modulo,
        _ => throw new BusinessRuleException("Tipo debe ser 'carrera_completa' o 'modulo'.")
    };

    private static string MapTipo(TipoPlanEstudio tipo) => tipo switch
    {
        TipoPlanEstudio.CarreraCompleta => "carrera_completa",
        TipoPlanEstudio.Modulo => "modulo",
        _ => throw new ArgumentOutOfRangeException(nameof(tipo))
    };

    private static PlanEstudioDto MapToDto(PlanEstudio planEstudio) => new()
    {
        Id = planEstudio.Id,
        CursoId = planEstudio.CursoId,
        Tipo = MapTipo(planEstudio.Tipo),
        DuracionMeses = planEstudio.DuracionMeses,
        PrecioMatricula = planEstudio.PrecioMatricula,
        PensionMensual = planEstudio.PensionMensual,
        Activo = planEstudio.Activo
    };
}
