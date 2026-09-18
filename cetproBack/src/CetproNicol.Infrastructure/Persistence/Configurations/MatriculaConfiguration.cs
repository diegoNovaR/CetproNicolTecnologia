using CetproNicol.Domain.Entities;
using CetproNicol.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CetproNicol.Infrastructure.Persistence.Configurations;

public class MatriculaConfiguration : IEntityTypeConfiguration<Matricula>
{
    private static readonly Dictionary<EstadoMatricula, string> ToDb = new()
    {
        [EstadoMatricula.Pendiente] = "pendiente",
        [EstadoMatricula.Activa] = "activa",
        [EstadoMatricula.Completada] = "completada",
        [EstadoMatricula.Cancelada] = "cancelada"
    };

    private static readonly Dictionary<string, EstadoMatricula> FromDb =
        ToDb.ToDictionary(kv => kv.Value, kv => kv.Key);

    public void Configure(EntityTypeBuilder<Matricula> builder)
    {
        builder.ToTable("matriculas");

        builder.HasKey(m => m.Id);

        builder.Property(m => m.Estado)
            .IsRequired()
            .HasMaxLength(20)
            .HasConversion(
                estado => ToDb[estado],
                valor => FromDb[valor]);

        builder.HasOne(m => m.Usuario)
            .WithMany(u => u.Matriculas)
            .HasForeignKey(m => m.UsuarioId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(m => m.PlanEstudio)
            .WithMany(p => p.Matriculas)
            .HasForeignKey(m => m.PlanEstudioId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(m => m.Pagos)
            .WithOne(p => p.Matricula)
            .HasForeignKey(p => p.MatriculaId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
