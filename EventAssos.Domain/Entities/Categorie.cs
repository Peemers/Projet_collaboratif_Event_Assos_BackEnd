namespace EventAssos.Domain.Entities;

public class Categorie
{
  public int Id { get; set; }
  public required string Nom { get; set; } = string.Empty;
  
  public ICollection<Evenement> Evenements { get; set; } = new List<Evenement>();
}