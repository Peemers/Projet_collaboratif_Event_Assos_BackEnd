using System.ComponentModel.DataAnnotations;

namespace EventAssos.Core.DTOs.Request.MembreRequestDtos;

public class LoginRequestDto
{
  [Required(ErrorMessage = "L'email est requis")]
  [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
    ErrorMessage = "Le format de l'email n'est pas valide")]
  [MaxLength(64, ErrorMessage = "64 Caractères Max")]
  public required string Email { get; set; }
  
  [Required(ErrorMessage = "Le password est requis")]
  [RegularExpression(@"^(?=.*[A-Z])(?=.*[!@#$%^&*(),.? "" :{}|<>+=\-])(?=.{6,16}$).*$")]
  [MinLength(6, ErrorMessage = "Taille min du password : 6 caractères")]
  [MaxLength(16, ErrorMessage = "Taille max du password : 16 caractères")]
  [DataType(DataType.Password)]
  public required string Password { get; set; }
}