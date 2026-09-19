using System.ComponentModel.DataAnnotations;

namespace CetproNicol.Application.DTOs;

public class CreatePlanEstudioDto
{
    [Required]
    public Guid CursoId { get; set; }

    [Required]
    [RegularExpression("^(carrera_completa|modulo)$", ErrorMessage = "Tipo debe ser 'carrera_completa' o 'modulo'.")]
    public string Tipo { get; set; } = string.Empty;

    [Range(1, 120)]
    public int DuracionMeses { get; set; }

    [Range(0, double.MaxValue)]
    public decimal PrecioMatricula { get; set; }

    [Range(0, double.MaxValue)]
    public decimal PensionMensual { get; set; }
}
