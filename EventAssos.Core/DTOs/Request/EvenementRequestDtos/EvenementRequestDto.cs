using System.ComponentModel.DataAnnotations;

namespace EventAssos.Core.DTOs.Request.EvenementRequestDtos;

public class EvenementRequestDto
{
  [Required(ErrorMessage = "Le nom est obligatoire")]
  [MaxLength(128)]
  public required string Nom { get; init; }
  
  [Required(ErrorMessage = "La déscription est obligatoire")]
  [MaxLength(150)]
  public required string Description { get; init; }
  
  [MaxLength(256)]
  public string? Lieu {get; init;}
  
  [Required(ErrorMessage = "La date de début est obligatoire")]
  public DateTime DateDebut { get; init; }
  
  [Required(ErrorMessage = "La date de fin est obligatoire")]
  public DateTime DateFin { get; init; }
  
  [Range(1, 200, ErrorMessage = "Le nombre de participants doit être entre 1 et 200")]
  public int NbMin  { get; init; }
  
  [Range(1, 200, ErrorMessage = "Le nombre de participants doit être entre 1 et 200")]
  public int NbMax { get; init; }
  
  public bool ListeAttenteActive { get; init; }
  
  // [Required(ErrorMessage = "La date d'inscription est obligatoire")]
  // public DateTime DateInscription { get; set; }
  
  [Required(ErrorMessage = "La date de limite d'inscription est obligatoire")]
  public DateTime DateLimiteInscription { get; init; }
  
  public List<int> CategorieIds { get; set; } = new(); //liste des id de categories choisies
}