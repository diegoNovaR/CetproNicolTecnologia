using CetproNicol.Application.DTOs;
using CetproNicol.Application.Exceptions;
using CetproNicol.Application.Interfaces;
using CetproNicol.Domain.Entities;
using CetproNicol.Domain.Enums;

namespace CetproNicol.Application.Services;

public class MatriculaService : IMatriculaService
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IPlanEstudioRepository _planEstudioRepository;
    private readonly IMatriculaRepository _matriculaRepository;
    private readonly IPagoRepository _pagoRepository;
    private readonly IUnitOfWork _unitOfWork;

    public MatriculaService(
        IUsuarioRepository usuarioRepository,
        IPlanEstudioRepository planEstudioRepository,
        IMatriculaRepository matriculaRepository,
        IPagoRepository pagoRepository,
        IUnitOfWork unitOfWork)
    {
        _usuarioRepository = usuarioRepository;
        _planEstudioRepository = planEstudioRepository;
        _matriculaRepository = matriculaRepository;
        _pagoRepository = pagoRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<MatriculaDto> MatricularEstudianteAsync(Guid usuarioId, Guid planEstudioId, CancellationToken cancellationToken = default)
    {
        var usuario = await _usuarioRepository.GetByIdAsync(usuarioId, cancellationToken)
            ?? throw new NotFoundException("El usuario no existe.");

        if (usuario.Rol != RolUsuario.Estudiante)
            throw new BusinessRuleException("El usuario no tiene rol de estudiante.");

        var planEstudio = await _planEstudioRepository.GetByIdAsync(planEstudioId, cancellationToken)
            ?? throw new NotFoundException("El plan de estudio no existe.");

        if (!planEstudio.Activo)
            throw new BusinessRuleException("El plan de estudio no está activo.");

        var yaTieneMatriculaActiva = await _matriculaRepository.ExisteMatriculaActivaParaCursoAsync(
            usuarioId, planEstudio.CursoId, cancellationToken);

        if (yaTieneMatriculaActiva)
            throw new BusinessRuleException("El usuario ya tiene una matrícula pendiente o activa para este curso.");

        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var ahora = DateTime.UtcNow;

            var matricula = new Matricula
            {
                Id = Guid.NewGuid(),
                UsuarioId = usuarioId,
                PlanEstudioId = planEstudioId,
                Estado = EstadoMatricula.Pendiente,
                FechaSolicitud = ahora
            };

            await _matriculaRepository.AddAsync(matricula, cancellationToken);

            // Cuota 0 es el pago único de matrícula; las cuotas 1..N son las pensiones mensuales.
            var pagos = new List<Pago>
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    MatriculaId = matricula.Id,
                    NumeroCuota = 0,
                    Periodo = ahora.ToString("yyyy-MM"),
                    Monto = planEstudio.PrecioMatricula,
                    Estado = EstadoPago.Pendiente
                }
            };

            for (var mes = 0; mes < planEstudio.DuracionMeses; mes++)
            {
                pagos.Add(new Pago
                {
                    Id = Guid.NewGuid(),
                    MatriculaId = matricula.Id,
                    NumeroCuota = mes + 1,
                    Periodo = ahora.AddMonths(mes).ToString("yyyy-MM"),
                    Monto = planEstudio.PensionMensual,
                    Estado = EstadoPago.Pendiente
                });
            }

            await _pagoRepository.AddRangeAsync(pagos, cancellationToken);

            await _unitOfWork.CommitTransactionAsync(cancellationToken);

            return MapToDto(matricula, pagos);
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }

    public async Task<PagoDto> AprobarPagoAsync(Guid pagoId, CancellationToken cancellationToken = default)
    {
        var pago = await _pagoRepository.GetByIdWithMatriculaAsync(pagoId, cancellationToken)
            ?? throw new NotFoundException("El pago no existe.");

        if (pago.Estado != EstadoPago.Pendiente)
            throw new BusinessRuleException("El pago no se encuentra en estado pendiente.");

        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var esElPrimerPagoAprobado = !await _pagoRepository.ExisteAlgunPagoAprobadoAsync(pago.MatriculaId, cancellationToken);

            var ahora = DateTime.UtcNow;
            pago.Estado = EstadoPago.Aprobado;
            pago.FechaPago = ahora;

            if (esElPrimerPagoAprobado && pago.Matricula.Estado == EstadoMatricula.Pendiente)
            {
                pago.Matricula.Estado = EstadoMatricula.Activa;
                pago.Matricula.FechaAprobacion = ahora;
            }

            await _unitOfWork.CommitTransactionAsync(cancellationToken);

            return MapToDto(pago);
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }

    public async Task<DeudaEstudianteDto> CalcularDeudaEstudianteAsync(Guid matriculaId, CancellationToken cancellationToken = default)
    {
        _ = await _matriculaRepository.GetByIdAsync(matriculaId, cancellationToken)
            ?? throw new NotFoundException("La matrícula no existe.");

        var pagosPendientes = await _pagoRepository.GetPendientesByMatriculaIdAsync(matriculaId, cancellationToken);

        return new DeudaEstudianteDto
        {
            MatriculaId = matriculaId,
            TotalDeuda = pagosPendientes.Sum(p => p.Monto),
            CuotasPendientes = pagosPendientes.Count,
            DetalleCuotas = pagosPendientes
                .Select(p => new CuotaPendienteDto { Periodo = p.Periodo, Monto = p.Monto })
                .ToList()
        };
    }

    public async Task<List<MatriculaResumenDto>> GetMatriculasAsync(Guid? usuarioId, CancellationToken cancellationToken = default)
    {
        var matriculas = await _matriculaRepository.GetAllConDetalleAsync(usuarioId, cancellationToken);
        return matriculas.Select(MapToResumenDto).ToList();
    }

    public async Task<List<PagoResumenDto>> GetPagosAsync(string? estado, CancellationToken cancellationToken = default)
    {
        EstadoPago? estadoFiltro = estado switch
        {
            null => null,
            "pendiente" => EstadoPago.Pendiente,
            "aprobado" => EstadoPago.Aprobado,
            _ => throw new BusinessRuleException("Estado debe ser 'pendiente' o 'aprobado'.")
        };

        var pagos = await _pagoRepository.GetAllConDetalleAsync(estadoFiltro, cancellationToken);
        return pagos.Select(MapToResumenDto).ToList();
    }

    private static MatriculaResumenDto MapToResumenDto(Matricula matricula) => new()
    {
        Id = matricula.Id,
        UsuarioId = matricula.UsuarioId,
        UsuarioNombreCompleto = $"{matricula.Usuario.Nombre} {matricula.Usuario.Apellido}",
        CursoId = matricula.PlanEstudio.CursoId,
        CursoNombre = matricula.PlanEstudio.Curso.Nombre,
        PlanEstudioId = matricula.PlanEstudioId,
        PlanTipo = MapTipoPlanEstudio(matricula.PlanEstudio.Tipo),
        Estado = MapEstadoMatricula(matricula.Estado),
        FechaSolicitud = matricula.FechaSolicitud,
        FechaAprobacion = matricula.FechaAprobacion
    };

    private static PagoResumenDto MapToResumenDto(Pago pago) => new()
    {
        Id = pago.Id,
        MatriculaId = pago.MatriculaId,
        UsuarioNombreCompleto = $"{pago.Matricula.Usuario.Nombre} {pago.Matricula.Usuario.Apellido}",
        CursoNombre = pago.Matricula.PlanEstudio.Curso.Nombre,
        NumeroCuota = pago.NumeroCuota,
        Periodo = pago.Periodo,
        Monto = pago.Monto,
        Estado = MapEstadoPago(pago.Estado),
        FechaPago = pago.FechaPago
    };

    private static string MapTipoPlanEstudio(TipoPlanEstudio tipo) => tipo switch
    {
        TipoPlanEstudio.CarreraCompleta => "carrera_completa",
        TipoPlanEstudio.Modulo => "modulo",
        _ => throw new ArgumentOutOfRangeException(nameof(tipo))
    };

    private static MatriculaDto MapToDto(Matricula matricula, IEnumerable<Pago> pagos) => new()
    {
        Id = matricula.Id,
        UsuarioId = matricula.UsuarioId,
        PlanEstudioId = matricula.PlanEstudioId,
        Estado = MapEstadoMatricula(matricula.Estado),
        FechaSolicitud = matricula.FechaSolicitud,
        FechaAprobacion = matricula.FechaAprobacion,
        Pagos = pagos.Select(MapToDto).ToList()
    };

    private static PagoDto MapToDto(Pago pago) => new()
    {
        Id = pago.Id,
        MatriculaId = pago.MatriculaId,
        NumeroCuota = pago.NumeroCuota,
        Periodo = pago.Periodo,
        Monto = pago.Monto,
        Estado = MapEstadoPago(pago.Estado),
        FechaPago = pago.FechaPago
    };

    private static string MapEstadoMatricula(EstadoMatricula estado) => estado switch
    {
        EstadoMatricula.Pendiente => "pendiente",
        EstadoMatricula.Activa => "activa",
        EstadoMatricula.Completada => "completada",
        EstadoMatricula.Cancelada => "cancelada",
        _ => throw new ArgumentOutOfRangeException(nameof(estado))
    };

    private static string MapEstadoPago(EstadoPago estado) => estado switch
    {
        EstadoPago.Pendiente => "pendiente",
        EstadoPago.Aprobado => "aprobado",
        _ => throw new ArgumentOutOfRangeException(nameof(estado))
    };
}
