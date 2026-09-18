using CetproNicol.Domain.Entities;
using CetproNicol.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CetproNicol.Infrastructure.Persistence.Configurations;

public class ConsultaConfiguration : IEntityTypeConfiguration<Consulta>
{
    private static readonly Dictionary<EstadoConsulta, string> ToDb = new()
    {
        [EstadoConsulta.Nueva] = "nueva",
        [EstadoConsulta.Contactada] = "contactada",
        [EstadoConsulta.Matriculada] = "matriculada"
    };

    private static readonly Dictionary<string, EstadoConsulta> FromDb =
        ToDb.ToDictionary(kv => kv.Value, kv => kv.Key);

    public void Configure(EntityTypeBuilder<Consulta> builder)
    {
        builder.ToTable("consultas");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.NombreContacto)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(c => c.Mensaje)
            .IsRequired();

        builder.Property(c => c.Origen)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(c => c.Estado)
            .IsRequired()
            .HasMaxLength(20)
            .HasConversion(
                estado => ToDb[estado],
                valor => FromDb[valor]);

        builder.HasOne(c => c.Area)
            .WithMany(a => a.Consultas)
            .HasForeignKey(c => c.AreaId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
