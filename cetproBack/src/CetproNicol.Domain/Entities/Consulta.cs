using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CetproNicol.Domain.Enums;

namespace CetproNicol.Domain.Entities;

public class Consulta
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(150)]
    public string NombreContacto { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string Telefono { get; set; } = string.Empty;

    [MaxLength(200)]
    [EmailAddress]
    public string? Email { get; set; }

    [ForeignKey(nameof(Area))]
    public Guid? AreaId { get; set; }

    [Required]
    public string Mensaje { get; set; } = string.Empty;

    [Required]
    public EstadoConsulta Estado { get; set; } = EstadoConsulta.Nueva;

    [Required]
    [MaxLength(50)]
    public string Origen { get; set; } = string.Empty;

    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

    public Area? Area { get; set; }
}
