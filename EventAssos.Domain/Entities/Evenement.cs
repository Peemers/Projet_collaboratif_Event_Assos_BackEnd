using EventAssos.Domain.Enums;

namespace EventAssos.Domain.Entities;

public class Evenement
{
  public Guid Id { get; set; } //PK
  public required string Nom { get; set; }
  public required string Description { get; set; }
  public string? Lieu { get; set; }
  public DateTime DateDebut { get; set; }
  public DateTime DateFin { get; set; }
  public int NbMin { get; set; }
  public int NbMax { get; set; }
  public StatutEvenement StatutEvenement { get; set; }
  public bool ListeAttenteActive { get; set; }
  public DateTime DateLimiteInscription { get; set; }
  public DateTime DateDeCreation { get; set; }
  public DateTime DateMaj { get; set; }

  public ICollection<Categorie> Categories { get; set; } = new List<Categorie>(); //new list par defaut pour éviter le null
  public ICollection<Inscription> Inscriptions { get; set; } = new List<Inscription>(); //idem
}


