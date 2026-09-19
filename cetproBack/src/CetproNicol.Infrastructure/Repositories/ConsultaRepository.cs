using CetproNicol.Application.Interfaces;
using CetproNicol.Domain.Entities;
using CetproNicol.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CetproNicol.Infrastructure.Repositories;

public class ConsultaRepository : IConsultaRepository
{
    private readonly CetproNicolDbContext _context;

    public ConsultaRepository(CetproNicolDbContext context)
    {
        _context = context;
    }

    public Task<List<Consulta>> GetAllAsync(CancellationToken cancellationToken = default) =>
        _context.Consultas.OrderByDescending(c => c.FechaCreacion).ToListAsync(cancellationToken);

    public async Task AddAsync(Consulta consulta, CancellationToken cancellationToken = default) =>
        await _context.Consultas.AddAsync(consulta, cancellationToken);
}
