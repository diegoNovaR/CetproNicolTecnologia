namespace CetproNicol.Application.DTOs;

public class PlanEstudioDto
{
    public Guid Id { get; set; }

    public Guid CursoId { get; set; }

    public string Tipo { get; set; } = string.Empty;

    public int DuracionMeses { get; set; }

    public decimal PrecioMatricula { get; set; }

    public decimal PensionMensual { get; set; }

    public bool Activo { get; set; }
}
