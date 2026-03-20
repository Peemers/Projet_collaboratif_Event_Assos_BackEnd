using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EventAssos.Domain.Entities;

namespace EventAssos.Infrastructure.DataBase.Configurations;

public class InscriptionConfiguration : IEntityTypeConfiguration<Inscription>
{
  public void Configure(EntityTypeBuilder<Inscription> builder)
  {
    builder.HasKey(i => new {i.MembreId, i.EvenementId}); //PK composite : une ligne avec deux identifiants

    builder.Property(i => i.InscriptionDate)
      .IsRequired();
    builder.Property(i => i.EstEnAttente)
      .IsRequired();
  }
}