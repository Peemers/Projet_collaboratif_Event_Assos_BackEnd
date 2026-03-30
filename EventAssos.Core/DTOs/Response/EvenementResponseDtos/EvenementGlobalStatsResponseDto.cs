namespace EventAssos.Core.DTOs.Response.EvenementResponseDtos;

public class EvenementGlobalStatsResponseDto
{
  public int TotalEvenements { get; init; }
  public List<CategorieCountDto> RepartitionCategories { get; set; } = new();
}

public class CategorieCountDto
{
  public string CategorieNom { get; set; } = string.Empty;
  public int CategorieNombre {get; set; }
}