using System.ComponentModel.DataAnnotations;

namespace CetproNicol.Application.DTOs;

public class MatricularEstudianteRequest
{
    [Required]
    public Guid UsuarioId { get; set; }

    [Required]
    public Guid PlanEstudioId { get; set; }
}
