using EventAssos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventAssos.Infrastructure.DataBase.Configurations;

public class EvenementConfiguration : IEntityTypeConfiguration<Evenement>
{
  public void Configure(EntityTypeBuilder<Evenement> builder)
  {
    builder.HasKey(e => e.Id);
    
    builder.Property(e => e.Nom)
      .IsRequired()
      .HasMaxLength(128);
    
    builder.Property(e => e.Description)
      .IsRequired()
      .HasMaxLength(512);
    
    builder.Property(e => e.Lieu)
      .HasMaxLength(128);
    
    builder.Property(e => e.DateDebut)
      .IsRequired()
      .HasColumnType("datetime");
    
    builder.Property(e => e.DateFin)
      .IsRequired()
      .HasColumnType("datetime");
    
    builder.Property(e => e.NbMin)
      .IsRequired()
      .HasColumnType("int");
    
    builder.Property(e => e.NbMax)
      .IsRequired()
      .HasColumnType("int");

    builder.Property(e => e.StatutEvenement)
      .IsRequired();
    
    builder.Property(e => e.ListeAttenteActive)
      .IsRequired();
    
    builder.Property(e => e.DateLimiteInscription)
      .IsRequired()
      .HasColumnType("datetime");
    
    builder.Property(e => e.DateDeCreation)
      .IsRequired()
      .HasColumnType("datetime")
      .HasDefaultValueSql("GETDATE()");
    
    builder.Property(e => e.DateMaj)
      .IsRequired()
      .HasColumnType("datetime");
    
    //relations Evenement <-> Inscription (1,n)
    
    builder.HasMany(e => e.Inscriptions) // Un event possede plusieurs inscription
      .WithOne(i => i.Evenement) // chaque inscription est liee à un seul event
      .HasForeignKey(i => i.EvenementId) //fk
      .OnDelete(DeleteBehavior.Cascade); //si on supprime un event on supprime aussi les inscriptions
    
    //relations Evenement <-> Categorie (n,n)
    
    builder.HasMany(e => e.Categories) // un event possede plusieurs categories
      .WithMany(c => c.Evenements) // une categorie appartient à plusieurs evenements
      .UsingEntity(j => j.ToTable("EvenementCategories")); //Cration de la table pivot ave ce nom
  }
}