using CetproNicol.Domain.Entities;
using CetproNicol.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CetproNicol.Infrastructure.Persistence.Configurations;

public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        builder.ToTable("usuarios");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(200);

        builder.HasIndex(u => u.Email)
            .IsUnique();

        builder.Property(u => u.Rol)
            .IsRequired()
            .HasMaxLength(20)
            .HasConversion(
                rol => rol == RolUsuario.Admin ? "admin" : "estudiante",
                valor => valor == "admin" ? RolUsuario.Admin : RolUsuario.Estudiante);

        builder.HasMany(u => u.Matriculas)
            .WithOne(m => m.Usuario)
            .HasForeignKey(m => m.UsuarioId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
