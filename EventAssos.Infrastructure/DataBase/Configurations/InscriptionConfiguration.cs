using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EventAssos.Domain.Entities;

namespace EventAssos.Infrastructure.DataBase.Configurations;

public class InscriptionConfiguration : IEntityTypeConfiguration<Inscription>
{
  public void Configure(EntityTypeBuilder<Inscription> builder)
  {
    
    builder.HasKey(i => i.Id);
    
    builder.HasOne(i => i.Membre)
      .WithMany(m => m.Inscriptions)
      .HasForeignKey(i => i.MembreId)
      .OnDelete(DeleteBehavior.Cascade);
    
    builder.HasOne(i => i.Evenement)
      .WithMany(e => e.Inscriptions)
      .HasForeignKey(i => i.EvenementId)
      .OnDelete(DeleteBehavior.Cascade);
    
    builder.HasIndex(i => new { i.MembreId, i.EvenementId }).IsUnique();
    
    builder.Property(i => i.InscriptionDate)
      .IsRequired();
  }
}