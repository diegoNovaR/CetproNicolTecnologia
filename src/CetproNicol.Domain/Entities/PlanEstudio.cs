using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CetproNicol.Domain.Enums;

namespace CetproNicol.Domain.Entities;

public class PlanEstudio
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [ForeignKey(nameof(Curso))]
    public Guid CursoId { get; set; }

    [Required]
    public TipoPlanEstudio Tipo { get; set; }

    public int DuracionMeses { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal PrecioMatricula { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal PensionMensual { get; set; }

    public bool Activo { get; set; } = true;

    public Curso Curso { get; set; } = null!;

    public ICollection<Matricula> Matriculas { get; set; } = new List<Matricula>();
}
