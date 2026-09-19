namespace CetproNicol.Application.DTOs;

public class MatriculaResumenDto
{
    public Guid Id { get; set; }

    public Guid UsuarioId { get; set; }

    public string UsuarioNombreCompleto { get; set; } = string.Empty;

    public Guid CursoId { get; set; }

    public string CursoNombre { get; set; } = string.Empty;

    public Guid PlanEstudioId { get; set; }

    public string PlanTipo { get; set; } = string.Empty;

    public string Estado { get; set; } = string.Empty;

    public DateTime FechaSolicitud { get; set; }

    public DateTime? FechaAprobacion { get; set; }
}
