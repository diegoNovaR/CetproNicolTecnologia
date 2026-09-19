namespace CetproNicol.Application.Interfaces;

public interface ITokenService
{
    string GenerateToken(Guid usuarioId, string email, string rol);
}
