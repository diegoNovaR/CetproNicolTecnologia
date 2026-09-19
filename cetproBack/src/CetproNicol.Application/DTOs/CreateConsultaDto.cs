using System.ComponentModel.DataAnnotations;

namespace CetproNicol.Application.DTOs;

public class CreateConsultaDto
{
    [Required]
    [MaxLength(150)]
    public string NombreContacto { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string Telefono { get; set; } = string.Empty;

    [EmailAddress]
    [MaxLength(200)]
    public string? Email { get; set; }

    public Guid? AreaId { get; set; }

    [Required]
    public string Mensaje { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string Origen { get; set; } = string.Empty;
}
