using System.ComponentModel.DataAnnotations;

namespace CetproNicol.Domain.Entities;

public class Area
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(150)]
    public string Nombre { get; set; } = string.Empty;

    public string? Descripcion { get; set; }

    [MaxLength(500)]
    public string? ImagenUrl { get; set; }

    public bool Activo { get; set; } = true;

    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

    public ICollection<Curso> Cursos { get; set; } = new List<Curso>();

    public ICollection<Consulta> Consultas { get; set; } = new List<Consulta>();
}
