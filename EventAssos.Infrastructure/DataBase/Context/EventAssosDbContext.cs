using EventAssos.Domain.Entities;
using EventAssos.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace EventAssos.Infrastructure.DataBase.Context;

public class EventAssosDbContext(DbContextOptions<EventAssosDbContext> options) : DbContext(options) //primary constructeur
{
  public DbSet<Categorie> Categories { get; set; } = null!;
  public DbSet<Evenement> Evenements { get; set; } = null!;
  public DbSet<Inscription> Inscriptions { get; set; } = null!;
  public DbSet<Membre> Membres { get; set; } = null!;

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.ApplyConfigurationsFromAssembly(typeof(EventAssosDbContext).Assembly);

    modelBuilder.Entity<Categorie>().HasData(
      new Categorie { Id = 1, Nom = "Concert" },
      new Categorie { Id = 2, Nom = "Conférence" },
      new Categorie { Id = 3, Nom = "Atelier" },
      new Categorie { Id = 4, Nom = "Autres" }
      );

    Guid adminGuid = Guid.Parse("11111111-1111-1111-1111-111111111111");

    modelBuilder.Entity<Membre>().HasData(
      new Membre
      {
        Id = adminGuid,
        Pseudo = "MmeDupont",
        Email = "admin@eventassos.com",
        Password = "$2a$11$0n/bZzK2T6K.wXyA3YtM.OR8.6.T.wXyA3YtM.OR8.6.T.wXyA3YtM.O",
        Role = Roles.Admin,
        Genre = Genres.Femme,
        DateNaissance = new DateTime(1978, 8, 13),
        DateInscription = new DateTime(2026, 03, 20),
      }
    );
  }
}