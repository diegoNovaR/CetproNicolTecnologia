using CetproNicol.Application.DTOs;

namespace CetproNicol.Application.Interfaces;

public interface IConsultaService
{
    Task<ConsultaDto> CrearAsync(CreateConsultaDto dto, CancellationToken cancellationToken = default);
}
