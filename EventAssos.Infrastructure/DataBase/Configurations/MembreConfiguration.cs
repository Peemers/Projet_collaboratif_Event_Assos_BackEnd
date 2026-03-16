using EventAssos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventAssos.Infrastructure.DataBase.Configurations;

public class MembreConfiguration : IEntityTypeConfiguration<Membre>
{
  public void Configure(EntityTypeBuilder<Membre> builder)
  {
    builder.HasKey(m => m.Id);

    builder.Property(m => m.Pseudo)
      .IsRequired()
      .HasMaxLength(35);
    
    builder.Property(m => m.Email)
      .IsRequired()
      .HasMaxLength(128);

    builder.Property(m => m.Password)
      .IsRequired()
      .HasMaxLength(255); // 255 pour avoir la place pour caser le password hash.

    builder.Property(m => m.Role)
      .IsRequired();
    
    builder.Property(m => m.Genre)
      .IsRequired();
    
    builder.HasIndex(m => m.Pseudo).IsUnique();
    builder.HasIndex(m => m.Email).IsUnique();
    
    //Relations
    
    builder.HasMany( m => m.Inscriptions) // Membre possede plusieurs inscriptions
      .WithOne( i => i.Membre) // Une inscription par membre
      .HasForeignKey(i => i.MembreId)
      .OnDelete(DeleteBehavior.Cascade); // si on supprime un membre, on supprime ses inscriptions avec.
    
  }
  
  
}