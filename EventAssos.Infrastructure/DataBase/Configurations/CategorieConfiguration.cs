using EventAssos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventAssos.Infrastructure.DataBase.Configurations;

public class CategorieConfiguration : IEntityTypeConfiguration<Categorie>
{
  public void Configure(EntityTypeBuilder<Categorie> builder)
  {
    builder.HasKey(c => c.Id);
    
    builder.Property(c => c.Nom)
      .IsRequired()
      .HasMaxLength(50);
    
    //pas de relation ici, deja faite avec EvenementConfiguration (Table Pivot) deja fait dans evenement avec ".using entity merci EFCore"
  }
}