using EventAssos.Core.DTOs.Request.MembreRequestDtos;
using EventAssos.Core.DTOs.Response;
using EventAssos.Core.Interfaces.Repositories;
using EventAssos.Core.Interfaces.Services;
using EventAssos.Core.Interfaces.Tools;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace EventAssos.Controllers;

[ApiController]
[Route("api/[controller]")]
[EnableRateLimiting("auth-limit")]
public class AuthController(IAuthService authService) : ControllerBase
{
  #region Register

  [HttpPost("register")]
  [EndpointSummary("Créer un nouveau membre / Register")]
  [EndpointDescription("Permet aux utilisateurs ou à l'admin de créer un nouveau compte")]
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

  #endregion

  #region Login

  [HttpPost("login")]
  [EndpointSummary("Se Connecter")]
  [EndpointDescription("Permet aux utilisateurs ou à l'admin de se connecter à leur(s) compte(s)")]
  public async Task<ActionResult<AuthResponseDto>> LoginAsync(LoginRequestDto loginDto)
  {
    try
    {
      AuthResponseDto result = await authService.LoginAsync(loginDto);
      return Ok(result);
    }
    catch (Exception e)
    {
      return BadRequest(e.Message);
    }
  }

  #endregion

  #region resetPassword
  [HttpPost("reset-password")]
  [EndpointSummary("Reinitialisation mot de passe")]
  [EndpointDescription("Permet à un utilisateur de recovery son mot de passe")]
  public async Task<IActionResult> ResetPassword(ResetPasswordDto dto)
  {
    try
    {
      await authService.ResetPasswordAsync(dto.Password);
      return Ok(dto.Password);
    }
    catch (Exception ex)
    {
      return BadRequest(ex.Message);
    }
  }
  #endregion
}

