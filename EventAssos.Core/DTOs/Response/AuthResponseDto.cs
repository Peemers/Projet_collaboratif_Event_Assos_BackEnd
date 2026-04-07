using EventAssos.Domain.Enums;

namespace EventAssos.Core.DTOs.Response;

public class AuthResponseDto
{
  public required string Token { get; init; }
  public required string Pseudo { get; init; }
  
  //public required Guid Id { get; init; } Proposition de modification pour pouvoir atteindre le role et l'id facilement
  
  //public required Roles Role {get; init; }
  
}