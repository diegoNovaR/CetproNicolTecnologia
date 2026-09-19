namespace CetproNicol.Application.DTOs;

public class ConsultaDto
{
    public Guid Id { get; set; }

    public string NombreContacto { get; set; } = string.Empty;

    public string Telefono { get; set; } = string.Empty;

    public string? Email { get; set; }

    public Guid? AreaId { get; set; }

    public string Mensaje { get; set; } = string.Empty;

    public string Estado { get; set; } = string.Empty;

    public string Origen { get; set; } = string.Empty;

    public DateTime FechaCreacion { get; set; }
}
