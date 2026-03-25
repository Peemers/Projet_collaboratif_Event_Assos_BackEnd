namespace EventAssos.Core.DTOs.Response.EvenementResponseDtos;

public class EvenementDetailsResponseDto
{
  public Guid Id { get; set; }
  public required string Nom { get; set; }
  public required string Description { get; set; }
  public string? Lieu {get; set;}
  public DateTime DateDebut { get; set; }
  public DateTime DateFin { get; set; }
  public int NbInscrits { get; set; } //prop calculee dans le mapper
  public int NbMin { get; set; }
  public int NbMax { get; set; }
  public required string Statut {get; set;} // string dans le dto pour faciliter le front aussi via le mapper
  public List<string> Categories { get; set; } = new();//string pour les noms uniquement
  
  public List<string> MembresInscrits { get; set; } = new(); //liste des membres inscrits, uniquement les pseudos 
  public List<string> ListeAttente = new(); //liste de ceux en attente : pseudo
  
  
  
  public DateTime DateLimiteInscription { get; set; }
  public bool ListeAttenteActive { get; set; }
}