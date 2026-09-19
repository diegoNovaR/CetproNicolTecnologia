using CetproNicol.Application.DTOs;
using CetproNicol.Application.Interfaces;
using CetproNicol.Domain.Enums;

namespace CetproNicol.Application.Services;

public class DashboardService : IDashboardService
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IAreaRepository _areaRepository;
    private readonly ICursoRepository _cursoRepository;
    private readonly IPlanEstudioRepository _planEstudioRepository;
    private readonly IMatriculaRepository _matriculaRepository;
    private readonly IPagoRepository _pagoRepository;
    private readonly IConsultaRepository _consultaRepository;

    public DashboardService(
        IUsuarioRepository usuarioRepository,
        IAreaRepository areaRepository,
        ICursoRepository cursoRepository,
        IPlanEstudioRepository planEstudioRepository,
        IMatriculaRepository matriculaRepository,
        IPagoRepository pagoRepository,
        IConsultaRepository consultaRepository)
    {
        _usuarioRepository = usuarioRepository;
        _areaRepository = areaRepository;
        _cursoRepository = cursoRepository;
        _planEstudioRepository = planEstudioRepository;
        _matriculaRepository = matriculaRepository;
        _pagoRepository = pagoRepository;
        _consultaRepository = consultaRepository;
    }

    public async Task<DashboardDto> GetResumenAsync(CancellationToken cancellationToken = default)
    {
        var usuarios = await _usuarioRepository.GetAllAsync(cancellationToken);
        var areas = await _areaRepository.GetAllAsync(cancellationToken);
        var cursos = await _cursoRepository.GetAllAsync(cancellationToken);
        var planesEstudio = await _planEstudioRepository.GetAllAsync(cancellationToken);
        var matriculas = await _matriculaRepository.GetAllAsync(cancellationToken);
        var pagos = await _pagoRepository.GetAllAsync(cancellationToken);
        var consultas = await _consultaRepository.GetAllAsync(cancellationToken);

        return new DashboardDto
        {
            TotalUsuarios = usuarios.Count,
            TotalEstudiantes = usuarios.Count(u => u.Rol == RolUsuario.Estudiante),

            TotalAreas = areas.Count,
            AreasActivas = areas.Count(a => a.Activo),

            TotalCursos = cursos.Count,
            CursosActivos = cursos.Count(c => c.Activo),

            TotalPlanesEstudio = planesEstudio.Count,

            TotalMatriculas = matriculas.Count,
            MatriculasPendientes = matriculas.Count(m => m.Estado == EstadoMatricula.Pendiente),
            MatriculasActivas = matriculas.Count(m => m.Estado == EstadoMatricula.Activa),
            MatriculasCompletadas = matriculas.Count(m => m.Estado == EstadoMatricula.Completada),
            MatriculasCanceladas = matriculas.Count(m => m.Estado == EstadoMatricula.Cancelada),

            PagosPendientes = pagos.Count(p => p.Estado == EstadoPago.Pendiente),
            PagosAprobados = pagos.Count(p => p.Estado == EstadoPago.Aprobado),
            MontoPendiente = pagos.Where(p => p.Estado == EstadoPago.Pendiente).Sum(p => p.Monto),
            MontoRecaudado = pagos.Where(p => p.Estado == EstadoPago.Aprobado).Sum(p => p.Monto),

            TotalConsultas = consultas.Count,
            ConsultasNuevas = consultas.Count(c => c.Estado == EstadoConsulta.Nueva)
        };
    }
}
