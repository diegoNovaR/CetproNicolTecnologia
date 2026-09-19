using CetproNicol.Application.DTOs;
using CetproNicol.Application.Exceptions;
using CetproNicol.Application.Interfaces;
using CetproNicol.Domain.Entities;
using CetproNicol.Domain.Enums;

namespace CetproNicol.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenService _tokenService;
    private readonly IUnitOfWork _unitOfWork;

    public AuthService(
        IUsuarioRepository usuarioRepository,
        IPasswordHasher passwordHasher,
        ITokenService tokenService,
        IUnitOfWork unitOfWork)
    {
        _usuarioRepository = usuarioRepository;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
        _unitOfWork = unitOfWork;
    }

    public async Task<AuthResponseDto> RegistrarAsync(RegistrarDto dto, CancellationToken cancellationToken = default)
    {
        var existente = await _usuarioRepository.GetByEmailAsync(dto.Email, cancellationToken);
        if (existente is not null)
            throw new BusinessRuleException("El email ya está registrado.");

        var usuario = new Usuario
        {
            Id = Guid.NewGuid(),
            Nombre = dto.Nombre,
            Apellido = dto.Apellido,
            Email = dto.Email,
            Telefono = dto.Telefono,
            PasswordHash = _passwordHasher.Hash(dto.Password),
            Rol = RolUsuario.Estudiante,
            FechaRegistro = DateTime.UtcNow
        };

        await _usuarioRepository.AddAsync(usuario, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return MapToDto(usuario);
    }

    public async Task<AuthResponseDto> LoginAsync(LoginDto dto, CancellationToken cancellationToken = default)
    {
        var usuario = await _usuarioRepository.GetByEmailAsync(dto.Email, cancellationToken)
            ?? throw new UnauthorizedException("Credenciales inválidas.");

        if (!_passwordHasher.Verify(dto.Password, usuario.PasswordHash))
            throw new UnauthorizedException("Credenciales inválidas.");

        return MapToDto(usuario);
    }

    private AuthResponseDto MapToDto(Usuario usuario)
    {
        var rol = MapRol(usuario.Rol);

        return new AuthResponseDto
        {
            Token = _tokenService.GenerateToken(usuario.Id, usuario.Email, rol),
            UsuarioId = usuario.Id,
            Nombre = usuario.Nombre,
            Apellido = usuario.Apellido,
            Email = usuario.Email,
            Rol = rol
        };
    }

    private static string MapRol(RolUsuario rol) => rol switch
    {
        RolUsuario.Admin => "admin",
        RolUsuario.Estudiante => "estudiante",
        _ => throw new ArgumentOutOfRangeException(nameof(rol))
    };
}
