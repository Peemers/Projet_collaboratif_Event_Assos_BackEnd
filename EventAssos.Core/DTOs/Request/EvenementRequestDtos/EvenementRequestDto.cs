using System.ComponentModel.DataAnnotations;

namespace EventAssos.Core.DTOs.Request.EvenementRequestDtos;

public class EvenementRequestDto
{
  [Required(ErrorMessage = "Le nom est obligatoire")]
  [MaxLength(128)]
  public required string Nom { get; set; }
  
  [Required(ErrorMessage = "La déscription est obligatoire")]
  [MaxLength(512)]
  public required string Description { get; set; }
  
  [MaxLength(256)]
  public string? Lieu {get; set;}
  
  [Required(ErrorMessage = "La date de début est obligatoire")]
  public DateTime DateDebut { get; set; }
  
  [Required(ErrorMessage = "La date de fin est obligatoire")]
  public DateTime DateFin { get; set; }
  
  [Range(1, 200, ErrorMessage = "Le nombre de participants doit être entre 1 et 200")]
  public int NbMin  { get; set; }
  
  [Range(1, 200, ErrorMessage = "Le nombre de participants doit être entre 1 et 200")]
  public int NbMax { get; set; }
  
  public required bool ListeAttenteActive { get; set; }
  
  [Required(ErrorMessage = "La date d'inscription est obligatoire")]
  public DateTime DateInscription { get; set; }
  
  [Required(ErrorMessage = "La date de limite d'inscription est obligatoire")]
  public DateTime DateLimiteInscription { get; set; }
  
  public List<int> CategorieIds { get; set; } = new();
}