using CetproNicol.Application.DTOs;
using CetproNicol.Application.Exceptions;
using CetproNicol.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CetproNicol.API.Controllers;

/// <summary>
/// Registro e inicio de sesión de usuarios. Endpoints públicos que emiten JWT.
/// </summary>
[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    /// <summary>
    /// Registra un nuevo usuario con rol "estudiante" y devuelve un JWT listo para usar.
    /// </summary>
    /// <param name="dto">Datos personales y contraseña del nuevo usuario.</param>
    /// <response code="201">Usuario creado; se devuelve el token y sus datos básicos.</response>
    /// <response code="400">El email ya está registrado o los datos son inválidos.</response>
    [HttpPost("registrar")]
    [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Registrar([FromBody] RegistrarDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var resultado = await _authService.RegistrarAsync(dto, cancellationToken);
            return StatusCode(StatusCodes.Status201Created, resultado);
        }
        catch (BusinessRuleException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Autentica un usuario por email y contraseña y devuelve un JWT con sus claims (userId, email, rol).
    /// </summary>
    /// <param name="dto">Email y contraseña.</param>
    /// <response code="200">Credenciales válidas; se devuelve el token y los datos del usuario.</response>
    /// <response code="401">Email o contraseña incorrectos.</response>
    [HttpPost("login")]
    [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var resultado = await _authService.LoginAsync(dto, cancellationToken);
            return Ok(resultado);
        }
        catch (UnauthorizedException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
    }
}
