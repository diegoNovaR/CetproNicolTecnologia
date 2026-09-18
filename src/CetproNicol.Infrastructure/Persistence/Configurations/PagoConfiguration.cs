using CetproNicol.Domain.Entities;
using CetproNicol.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CetproNicol.Infrastructure.Persistence.Configurations;

public class PagoConfiguration : IEntityTypeConfiguration<Pago>
{
    public void Configure(EntityTypeBuilder<Pago> builder)
    {
        builder.ToTable("pagos");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Periodo)
            .IsRequired()
            .HasMaxLength(7);

        builder.Property(p => p.Estado)
            .IsRequired()
            .HasMaxLength(20)
            .HasConversion(
                estado => estado == EstadoPago.Aprobado ? "aprobado" : "pendiente",
                valor => valor == "aprobado" ? EstadoPago.Aprobado : EstadoPago.Pendiente);

        builder.HasOne(p => p.Matricula)
            .WithMany(m => m.Pagos)
            .HasForeignKey(p => p.MatriculaId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
