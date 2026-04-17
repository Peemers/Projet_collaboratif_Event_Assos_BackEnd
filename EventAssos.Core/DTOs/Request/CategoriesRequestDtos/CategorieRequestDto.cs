using System.ComponentModel.DataAnnotations;

namespace EventAssos.Core.DTOs.Request.CategoriesRequestDtos;

public class CategorieRequestDto
{
  [Required(ErrorMessage = "Le nom de la categorie est obligatoire")]
  [MaxLength(50, ErrorMessage = "Le nom ne peut dépasser 50 caractères ")]
  [MinLength(3, ErrorMessage = "Minimum 3 caractères dans le nom de la categorie ")]
  public required string Nom { get; set; }
}