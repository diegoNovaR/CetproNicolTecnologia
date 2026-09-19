namespace CetproNicol.Application.DTOs;

public class DashboardDto
{
    public int TotalUsuarios { get; set; }

    public int TotalEstudiantes { get; set; }

    public int TotalAreas { get; set; }

    public int AreasActivas { get; set; }

    public int TotalCursos { get; set; }

    public int CursosActivos { get; set; }

    public int TotalPlanesEstudio { get; set; }

    public int TotalMatriculas { get; set; }

    public int MatriculasPendientes { get; set; }

    public int MatriculasActivas { get; set; }

    public int MatriculasCompletadas { get; set; }

    public int MatriculasCanceladas { get; set; }

    public int PagosPendientes { get; set; }

    public int PagosAprobados { get; set; }

    public decimal MontoPendiente { get; set; }

    public decimal MontoRecaudado { get; set; }

    public int TotalConsultas { get; set; }

    public int ConsultasNuevas { get; set; }
}
