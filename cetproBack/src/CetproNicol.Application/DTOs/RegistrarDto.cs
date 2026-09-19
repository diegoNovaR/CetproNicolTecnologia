using System.ComponentModel.DataAnnotations;

namespace CetproNicol.Application.DTOs;

public class RegistrarDto
{
    [Required]
    [MaxLength(100)]
    public string Nombre { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Apellido { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [MaxLength(200)]
    public string Email { get; set; } = string.Empty;

    [MaxLength(20)]
    public string? Telefono { get; set; }

    [Required]
    [MinLength(8)]
    public string Password { get; set; } = string.Empty;
}
