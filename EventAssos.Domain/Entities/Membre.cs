using EventAssos.Domain.Enums;

namespace EventAssos.Domain.Entities;

public class Membre
{
  public required Guid Id { get; set; } //PK
  public required string Pseudo { get; set; }
  public required string Email { get; set; }
  public required string Password { get; set; }
  public required Roles Role { get; set; }
  public required Genres Genre { get; set; }
  public DateTime DateNaissance { get; set; }

  public ICollection<Inscription> Inscriptions { get; set; } = new List<Inscription>();
}