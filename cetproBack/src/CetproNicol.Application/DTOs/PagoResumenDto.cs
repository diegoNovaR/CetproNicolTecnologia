namespace CetproNicol.Application.DTOs;

public class PagoResumenDto
{
    public Guid Id { get; set; }

    public Guid MatriculaId { get; set; }

    public string UsuarioNombreCompleto { get; set; } = string.Empty;

    public string CursoNombre { get; set; } = string.Empty;

    public int NumeroCuota { get; set; }

    public string Periodo { get; set; } = string.Empty;

    public decimal Monto { get; set; }

    public string Estado { get; set; } = string.Empty;

    public DateTime? FechaPago { get; set; }
}
