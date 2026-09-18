namespace CetproNicol.Application.DTOs;

public class PagoDto
{
    public Guid Id { get; set; }

    public Guid MatriculaId { get; set; }

    public int NumeroCuota { get; set; }

    public string Periodo { get; set; } = string.Empty;

    public decimal Monto { get; set; }

    public string Estado { get; set; } = string.Empty;

    public DateTime? FechaPago { get; set; }
}
