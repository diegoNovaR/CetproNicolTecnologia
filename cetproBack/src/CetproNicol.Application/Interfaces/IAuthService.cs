using CetproNicol.Application.DTOs;

namespace CetproNicol.Application.Interfaces;

public interface IAuthService
{
    Task<AuthResponseDto> RegistrarAsync(RegistrarDto dto, CancellationToken cancellationToken = default);

    Task<AuthResponseDto> LoginAsync(LoginDto dto, CancellationToken cancellationToken = default);
}
