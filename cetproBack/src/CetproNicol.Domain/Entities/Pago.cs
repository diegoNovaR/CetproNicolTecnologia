using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CetproNicol.Domain.Enums;

namespace CetproNicol.Domain.Entities;

public class Pago
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [ForeignKey(nameof(Matricula))]
    public Guid MatriculaId { get; set; }

    public int NumeroCuota { get; set; }

    [Required]
    [MaxLength(7)]
    public string Periodo { get; set; } = string.Empty;

    [Column(TypeName = "decimal(10,2)")]
    public decimal Monto { get; set; }

    [Required]
    public EstadoPago Estado { get; set; } = EstadoPago.Pendiente;

    public DateTime? FechaPago { get; set; }

    public Matricula Matricula { get; set; } = null!;
}
