using EventAssos.Core.DTOs.Request.MembreRequestDtos;
using EventAssos.Core.DTOs.Response;

namespace EventAssos.Core.Interfaces.Services;

public interface IAuthService
{
  Task<AuthResponseDto> LoginAsync(LoginRequestDto loginDto);
  
  Task<AuthResponseDto> RegisterAsync(RegisterRequestDto registerDto);
}