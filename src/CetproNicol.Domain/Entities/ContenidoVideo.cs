using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CetproNicol.Domain.Entities;

public class ContenidoVideo
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [ForeignKey(nameof(Curso))]
    public Guid CursoId { get; set; }

    [Required]
    [MaxLength(200)]
    public string Titulo { get; set; } = string.Empty;

    public string? Descripcion { get; set; }

    [Required]
    [MaxLength(500)]
    public string UrlVideo { get; set; } = string.Empty;

    public int Orden { get; set; }

    public bool Activo { get; set; } = true;

    public Curso Curso { get; set; } = null!;
}
