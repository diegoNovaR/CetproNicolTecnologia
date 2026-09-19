namespace CetproNicol.Application.DTOs;

public class AuthResponseDto
{
    public string Token { get; set; } = string.Empty;

    public Guid UsuarioId { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public string Apellido { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Rol { get; set; } = string.Empty;
}
