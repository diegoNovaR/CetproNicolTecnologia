using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace CetproNicol.API.Hubs;

[Authorize]
public class NotificacionesHub : Hub
{
    public const string GrupoAdmins = "admins";

    public static string GrupoUsuario(string usuarioId) => $"user-{usuarioId}";

    public override async Task OnConnectedAsync()//statefull 
    {
        var usuario = Context.User;
        var rol = usuario?.FindFirst(ClaimTypes.Role)?.Value;

        if (rol == "admin")
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, GrupoAdmins);//aqui guardamos el estado 
        }
        else
        {
            var usuarioId = usuario?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!string.IsNullOrEmpty(usuarioId))
                await Groups.AddToGroupAsync(Context.ConnectionId, GrupoUsuario(usuarioId));
        }

        await base.OnConnectedAsync();
    }
}
