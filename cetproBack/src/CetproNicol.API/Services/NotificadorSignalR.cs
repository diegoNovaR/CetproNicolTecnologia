using CetproNicol.API.Hubs;
using CetproNicol.Application.DTOs;
using CetproNicol.Application.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace CetproNicol.API.Services;

public class NotificadorSignalR : INotificador
{
    private readonly IHubContext<NotificacionesHub> _hubContext;

    public NotificadorSignalR(IHubContext<NotificacionesHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public Task NotificarNuevaMatriculaAsync(MatriculaDto matricula) =>
        _hubContext.Clients.Group(NotificacionesHub.GrupoAdmins).SendAsync("NuevaMatricula", matricula);

    public Task NotificarPagoAprobadoAsync(Guid usuarioId, PagoDto pago, string estadoMatricula) =>
        _hubContext.Clients.Group(NotificacionesHub.GrupoUsuario(usuarioId.ToString())).SendAsync("PagoAprobado", pago, estadoMatricula);
}
