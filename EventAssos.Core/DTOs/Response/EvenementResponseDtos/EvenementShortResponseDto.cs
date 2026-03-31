namespace EventAssos.Core.DTOs.Response.EvenementResponseDtos;

public class EvenementShortResponseDto
{
  public Guid Id { get; init; }
  public required string Nom { get; init; }
  public required string Description { get; init; }
  public string? Lieu {get; init;}
  public DateTime DateDebut { get; init; }
  public DateTime DateFin { get; init; }
  public int NbInscrits { get; init; } //prop calculee dans le mapper
  public int NbMin { get; init; }
  public int NbMax { get; init; }
  public List<string> Categories { get; set; } = new();//string pour les noms uniquement
  public required string Statut {get; init;} // string dans le dto pour faciliter le front aussi via le mapper
  public DateTime DateLimiteInscription { get; init; }
  public bool ListeAttenteActive { get; init; }
}