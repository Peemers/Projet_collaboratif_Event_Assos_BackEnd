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
  
  public required string Token { get; init; }
  public required string Email { get; init; }
}