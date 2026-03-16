using EventAssos.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventAssos.Infrastructure.DataBase.Context;

public class EventAssosDbContext : DbContext
{
  public EventAssosDbContext(DbContextOptions<EventAssosDbContext> options) : base(options)
  {
  }

  public DbSet<Categorie> Categories { get; set; } = null!;
  public DbSet<Evenement> Evenements { get; set; } = null!;
  public DbSet<Inscription> Inscriptions { get; set; } = null!;
  public DbSet<Membre> Membres { get; set; } = null!;

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.ApplyConfigurationsFromAssembly(typeof(EventAssosDbContext).Assembly);
  }

}