using System.ComponentModel.DataAnnotations;
using CetproNicol.Domain.Enums;

namespace CetproNicol.Domain.Entities;

public class Usuario
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Nombre { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Apellido { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [MaxLength(20)]
    public string? Telefono { get; set; }

    [Required]
    public string PasswordHash { get; set; } = string.Empty;

    [Required]
    public RolUsuario Rol { get; set; } = RolUsuario.Estudiante;

    public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;

    public ICollection<Matricula> Matriculas { get; set; } = new List<Matricula>();
}
