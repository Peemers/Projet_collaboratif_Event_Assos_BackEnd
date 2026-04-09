using System.ComponentModel.DataAnnotations;

namespace EventAssos.Core.DTOs.Request.MembreRequestDtos;

public class ResetPasswordDto
{
  [Required (ErrorMessage = "Un nouveau password est requis pour le changement")]
  [RegularExpression(@"^(?=.*[A-Z])(?=.*[!@#$%^&*(),.? "" :{}|<>+=\-])(?=.{6,16}$).*$", ErrorMessage = 
    "Format de mot de passe invalide (Une majuscule minimum et un caractere spécial minimum")]
  [MinLength(6, ErrorMessage = "Minimum 6 Caracteres")]
  [MaxLength(16, ErrorMessage = "Maximum 16 Caracteres")]
  [DataType(DataType.Password)]
  public required string Password { get; init; }
  
  [Required(ErrorMessage = "Token Requis")]
  public required string Token { get; init; }
  
  [Required(ErrorMessage = "Email Requis")]
  [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
    ErrorMessage = "Le format de l'email n'est pas valide")]
  [MaxLength(64, ErrorMessage = "Maximum 64 Caracteres")]
  public required string Email { get; init; }
}