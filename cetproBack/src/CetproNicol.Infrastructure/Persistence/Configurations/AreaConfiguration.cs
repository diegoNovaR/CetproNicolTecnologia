using CetproNicol.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CetproNicol.Infrastructure.Persistence.Configurations;

public class AreaConfiguration : IEntityTypeConfiguration<Area>
{
    public void Configure(EntityTypeBuilder<Area> builder)
    {
        builder.ToTable("areas");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Nombre)
            .IsRequired()
            .HasMaxLength(150);

        builder.HasMany(a => a.Cursos)
            .WithOne(c => c.Area)
            .HasForeignKey(c => c.AreaId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(a => a.Consultas)
            .WithOne(c => c.Area)
            .HasForeignKey(c => c.AreaId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
