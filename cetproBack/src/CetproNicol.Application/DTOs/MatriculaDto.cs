namespace CetproNicol.Application.DTOs;

public class MatriculaDto
{
    public Guid Id { get; set; }

    public Guid UsuarioId { get; set; }

    public Guid PlanEstudioId { get; set; }

    public string Estado { get; set; } = string.Empty;

    public DateTime FechaSolicitud { get; set; }

    public DateTime? FechaAprobacion { get; set; }

    public List<PagoDto> Pagos { get; set; } = new();
}
