using System.ComponentModel.DataAnnotations;
using EventAssos.Domain.Enums;

namespace EventAssos.Core.DTOs.Request.MembreRequestDtos;

public class RegisterRequestDto
{
  [Required]
  [MinLength(4, ErrorMessage = "Taille min du pseudo : 4 caractères")]
  [MaxLength(24, ErrorMessage = "Taille max du pseudo : 24 caractère")]
  public required string Pseudo { get; init; }
  
  [Required(ErrorMessage = "L'email est requis")]
  [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
    ErrorMessage = "Le format de l'email n'est pas valide")]
  [MaxLength(64, ErrorMessage = "64 Caractères Max")]
  public required string Email { get; init; }
  
  [Required(ErrorMessage = "Le password est requis")]
  [RegularExpression(@"^(?=.*[A-Z])(?=.*[!@#$%^&*(),.? "" :{}|<>+=\-])(?=.{6,16}$).*$")]
  [MinLength(6, ErrorMessage = "Taille min du password : 6 caractères")]
  [MaxLength(16, ErrorMessage = "Taille max du password : 16 caractères")]
  [DataType(DataType.Password)]
  public required string Password { get; init; }
  
  [Required(ErrorMessage = "Le genre est requis")]
  public required Genres Genre  { get; init; }
  
  [DataType(DataType.Date)]
  public DateTime DateNaissance { get; init; }
}