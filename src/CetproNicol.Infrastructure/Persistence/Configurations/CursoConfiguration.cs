using CetproNicol.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CetproNicol.Infrastructure.Persistence.Configurations;

public class CursoConfiguration : IEntityTypeConfiguration<Curso>
{
    public void Configure(EntityTypeBuilder<Curso> builder)
    {
        builder.ToTable("cursos");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Nombre)
            .IsRequired()
            .HasMaxLength(150);

        builder.HasOne(c => c.Area)
            .WithMany(a => a.Cursos)
            .HasForeignKey(c => c.AreaId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(c => c.PlanesEstudio)
            .WithOne(p => p.Curso)
            .HasForeignKey(p => p.CursoId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(c => c.ContenidosVideo)
            .WithOne(cv => cv.Curso)
            .HasForeignKey(cv => cv.CursoId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
