using CetproNicol.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CetproNicol.Infrastructure.Persistence;

public class CetproNicolDbContext : DbContext
{
    public CetproNicolDbContext(DbContextOptions<CetproNicolDbContext> options) : base(options)
    {
    }

    public DbSet<Area> Areas => Set<Area>();

    public DbSet<Curso> Cursos => Set<Curso>();

    public DbSet<PlanEstudio> PlanesEstudio => Set<PlanEstudio>();

    public DbSet<Usuario> Usuarios => Set<Usuario>();

    public DbSet<Matricula> Matriculas => Set<Matricula>();

    public DbSet<Pago> Pagos => Set<Pago>();

    public DbSet<ContenidoVideo> ContenidosVideo => Set<ContenidoVideo>();

    public DbSet<Consulta> Consultas => Set<Consulta>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CetproNicolDbContext).Assembly);
    }
}
