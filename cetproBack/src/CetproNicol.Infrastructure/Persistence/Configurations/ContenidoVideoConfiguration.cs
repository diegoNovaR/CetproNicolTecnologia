using CetproNicol.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CetproNicol.Infrastructure.Persistence.Configurations;

public class ContenidoVideoConfiguration : IEntityTypeConfiguration<ContenidoVideo>
{
    public void Configure(EntityTypeBuilder<ContenidoVideo> builder)
    {
        builder.ToTable("contenidos_video");

        builder.HasKey(cv => cv.Id);

        builder.Property(cv => cv.Titulo)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(cv => cv.UrlVideo)
            .IsRequired()
            .HasMaxLength(500);

        builder.HasOne(cv => cv.Curso)
            .WithMany(c => c.ContenidosVideo)
            .HasForeignKey(cv => cv.CursoId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
