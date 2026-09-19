using CetproNicol.Application.Interfaces;
using CetproNicol.Domain.Entities;
using CetproNicol.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CetproNicol.Infrastructure.Repositories;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly CetproNicolDbContext _context;

    public UsuarioRepository(CetproNicolDbContext context)
    {
        _context = context;
    }

    public Task<List<Usuario>> GetAllAsync(CancellationToken cancellationToken = default) =>
        _context.Usuarios.ToListAsync(cancellationToken);

    public Task<Usuario?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        _context.Usuarios.FirstOrDefaultAsync(u => u.Id == id, cancellationToken);

    public Task<Usuario?> GetByEmailAsync(string email, CancellationToken cancellationToken = default) =>
        _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);

    public async Task AddAsync(Usuario usuario, CancellationToken cancellationToken = default) =>
        await _context.Usuarios.AddAsync(usuario, cancellationToken);
}
