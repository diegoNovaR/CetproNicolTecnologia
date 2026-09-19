namespace CetproNicol.Application.DTOs;

public class CursoDto
{
    public Guid Id { get; set; }

    public Guid AreaId { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public string? Descripcion { get; set; }

    public string? ImagenUrl { get; set; }

    public bool Activo { get; set; }

    public DateTime FechaCreacion { get; set; }
}
