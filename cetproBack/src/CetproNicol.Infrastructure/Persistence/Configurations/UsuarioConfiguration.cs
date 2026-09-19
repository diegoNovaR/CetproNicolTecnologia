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

        // Contraseña semilla: "Admin123!" (cambiarla tras el primer login).
        builder.HasData(new Usuario
        {
            Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
            Nombre = "Admin",
            Apellido = "CetproNicol",
            Email = "admin@cetpronicol.com",
            Telefono = null,
            PasswordHash = "$2a$11$h0/WqTJnQFJNVvDj9ofT5eQt4JSwlI2ZG1tm2MtcUOU.ghOpnNOeK",
            Rol = RolUsuario.Admin,
            FechaRegistro = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        });
    }
}
