namespace CetproNicol.Application.DTOs;

public class DeudaEstudianteDto
{
    public Guid MatriculaId { get; set; }

    public decimal TotalDeuda { get; set; }

    public int CuotasPendientes { get; set; }

    public List<CuotaPendienteDto> DetalleCuotas { get; set; } = new();
}
