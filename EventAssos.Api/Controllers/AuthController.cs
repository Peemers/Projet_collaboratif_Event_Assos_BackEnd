using EventAssos.Core.DTOs.Request.MembreRequestDtos;
using EventAssos.Core.DTOs.Response;
using EventAssos.Core.Interfaces.Repositories;
using EventAssos.Core.Interfaces.Services;
using EventAssos.Core.Interfaces.Tools;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventAssos.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService authService) : ControllerBase
{
  [HttpPost("register")]
  public async Task<ActionResult<AuthResponseDto>> RegisterAsync(RegisterRequestDto registerDto)
  {
    try
    {
      AuthResponseDto result = await authService.RegisterAsync(registerDto);
      return Ok(result);
    }
    catch (Exception ex)
    {
      return BadRequest(ex.Message);
    }
  }

  [HttpPost("login")]
  public async Task<ActionResult<AuthResponseDto>> LoginAsync(LoginRequestDto loginDto)
  {
    try
    {
      AuthResponseDto result = await authService.LoginAsync(loginDto);
      return Ok(result);
    }
    catch (Exception e)
    {
      return Unauthorized(e.Message);
    }
  }
}