using System.ComponentModel.DataAnnotations;

namespace EventAssos.Core.DTOs.Request.MembreRequestDtos;

public class PasswordPerduRequestDto
{
  [Required(ErrorMessage = "L'e-mail est obligatoire")]
  [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
    ErrorMessage = "Le format de l'email n'est pas valide")]
  [MaxLength(64, ErrorMessage = "Maximum 64 Caracteres")]
  [DataType(DataType.EmailAddress)]
  public required string Email { get; init; }
}