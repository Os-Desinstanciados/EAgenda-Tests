using eAgenda.Dominio.Modulos.ModuloInstituicao;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eAgenda.Infra.Modulos.ModuloInstituicao;

public sealed class InstituicaoConfiguration : IEntityTypeConfiguration<Instituicao>
{
    public void Configure(EntityTypeBuilder<Instituicao> builder)
    {
        builder.ToTable("TBInstituicao");

        builder.HasKey(i => i.UserId)
            .HasName("PK_TBInstituicao");

        builder.Property(i => i.UserId)
            .ValueGeneratedNever();

        builder.Property(i => i.Nome)
            .HasMaxLength(100)
            .IsRequired();

        builder.HasOne<IdentityUser<Guid>>()
            .WithOne()
            .HasForeignKey<Instituicao>(i => i.UserId)
            .HasConstraintName("FK_TBInstituicao_AspNetUsers")
            .OnDelete(DeleteBehavior.Restrict);
    }
}