namespace EventAssos.Core.DTOs.Response.EvenementResponseDtos;

public class EvenementStatsResponseDto
{
  public Guid Id { get; init; }
  public string Nom { get; init; } = string.Empty;
  public int NbMin { get; init; } 
  public int NbMax { get; init; }
  public int NbInscrits { get; init; }
  public int NbListeAttente { get; init; }
  public double TauxRemplissage { get; init; }
  public bool EstViable { get; init; }
}