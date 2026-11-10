using CetproNicol.Application.DTOs;

namespace CetproNicol.Application.Interfaces;

public interface INotificador
{
    Task NotificarNuevaMatriculaAsync(MatriculaDto matricula);

    Task NotificarPagoAprobadoAsync(Guid usuarioId, PagoDto pago, string estadoMatricula);
}
