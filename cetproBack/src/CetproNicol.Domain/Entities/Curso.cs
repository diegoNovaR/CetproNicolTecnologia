using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CetproNicol.Domain.Entities;

public class Curso
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [ForeignKey(nameof(Area))]
    public Guid AreaId { get; set; }

    [Required]
    [MaxLength(150)]
    public string Nombre { get; set; } = string.Empty;

    public string? Descripcion { get; set; }

    [MaxLength(500)]
    public string? ImagenUrl { get; set; }

    public bool Activo { get; set; } = true;

    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

    public Area Area { get; set; } = null!;

    public ICollection<PlanEstudio> PlanesEstudio { get; set; } = new List<PlanEstudio>();

    public ICollection<ContenidoVideo> ContenidosVideo { get; set; } = new List<ContenidoVideo>();
}
