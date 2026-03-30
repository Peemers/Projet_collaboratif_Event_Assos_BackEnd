using EventAssos.Core.DTOs.Request.CategoriesRequestDtos;
using EventAssos.Core.DTOs.Response.CategorieResponseDtos;
using EventAssos.Domain.Entities;

namespace EventAssos.Core.Mappers;

public static class CategorieMapper
{
  public static CategorieResponseDto ToResponseDto(this Categorie cat) => new()
  {
    Id = cat.Id,
    Nom = cat.Nom,
  };

  public static Categorie ToEntity(this CategorieRequestDto cat) => new()
  {
    Nom = cat.Nom,
  };
}