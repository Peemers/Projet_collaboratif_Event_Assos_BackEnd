namespace EventAssos.Domain.Entities;

public class Inscription
{
  public required Guid MembreId { get; set; } //fk
  public Membre Membre { get; set; } = null!;
  
  public required Guid EvenementId { get; set; } //fk
  public Evenement Evenement { get; set; } = null!;
  
  public DateTime InscriptionDate { get; set; }
  public bool EstEnAttente { get; set; }
}