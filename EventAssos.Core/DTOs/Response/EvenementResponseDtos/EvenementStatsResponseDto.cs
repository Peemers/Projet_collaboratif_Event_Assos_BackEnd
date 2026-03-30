namespace EventAssos.Core.DTOs.Response.EvenementResponseDtos;

public class EvenementStatsResponseDto
{
  public Guid Id { get; set; }
  public string Nom { get; set; } = string.Empty;
  public int NbMin { get; set; } 
  public int NbMax { get; set; }
  public int NbInscrits { get; set; }
  public int NbListeAttente { get; set; }
  public double TauxRemplissage { get; set; }
  public bool EstViable { get; set; }
}