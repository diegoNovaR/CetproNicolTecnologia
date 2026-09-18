using Microsoft.EntityFrameworkCore;

namespace CetproNicol.Infrastructure.Persistence;

public class CetproNicolDbContext : DbContext
{
    public CetproNicolDbContext(DbContextOptions<CetproNicolDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CetproNicolDbContext).Assembly);
    }
}
