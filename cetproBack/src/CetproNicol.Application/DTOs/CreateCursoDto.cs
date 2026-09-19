using System.ComponentModel.DataAnnotations;

namespace CetproNicol.Application.DTOs;

public class CreateCursoDto
{
    [Required]
    public Guid AreaId { get; set; }

    [Required]
    [MaxLength(150)]
    public string Nombre { get; set; } = string.Empty;

    public string? Descripcion { get; set; }

    [MaxLength(500)]
    public string? ImagenUrl { get; set; }
}
