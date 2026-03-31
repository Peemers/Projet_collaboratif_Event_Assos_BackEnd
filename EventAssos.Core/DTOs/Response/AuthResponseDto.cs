namespace EventAssos.Core.DTOs.Response;

public class AuthResponseDto
{
  public required string Token { get; init; }
  public required string Pseudo { get; init; }
}