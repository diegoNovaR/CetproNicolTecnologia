using CetproNicol.Domain.Entities;
using CetproNicol.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CetproNicol.Infrastructure.Persistence.Configurations;

public class PlanEstudioConfiguration : IEntityTypeConfiguration<PlanEstudio>
{
    public void Configure(EntityTypeBuilder<PlanEstudio> builder)
    {
        builder.ToTable("planes_estudio");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Tipo)
            .IsRequired()
            .HasMaxLength(20)
            .HasConversion(
                tipo => tipo == TipoPlanEstudio.CarreraCompleta ? "carrera_completa" : "modulo",
                valor => valor == "carrera_completa" ? TipoPlanEstudio.CarreraCompleta : TipoPlanEstudio.Modulo);

        builder.HasOne(p => p.Curso)
            .WithMany(c => c.PlanesEstudio)
            .HasForeignKey(p => p.CursoId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.Matriculas)
            .WithOne(m => m.PlanEstudio)
            .HasForeignKey(m => m.PlanEstudioId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
