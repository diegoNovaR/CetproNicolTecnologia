using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CetproNicol.Domain.Enums;

namespace CetproNicol.Domain.Entities;

public class Matricula
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [ForeignKey(nameof(Usuario))]
    public Guid UsuarioId { get; set; }

    [Required]
    [ForeignKey(nameof(PlanEstudio))]
    public Guid PlanEstudioId { get; set; }

    [Required]
    public EstadoMatricula Estado { get; set; } = EstadoMatricula.Pendiente;

    public DateTime FechaSolicitud { get; set; } = DateTime.UtcNow;

    public DateTime? FechaAprobacion { get; set; }

    public Usuario Usuario { get; set; } = null!;

    public PlanEstudio PlanEstudio { get; set; } = null!;

    public ICollection<Pago> Pagos { get; set; } = new List<Pago>();
}
