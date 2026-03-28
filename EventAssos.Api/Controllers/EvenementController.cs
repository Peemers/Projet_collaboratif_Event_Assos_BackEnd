using EventAssos.Core.DTOs.Request.EvenementRequestDtos;
using EventAssos.Core.DTOs.Response.EvenementResponseDtos;
using EventAssos.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace EventAssos.Controllers;



[Route("api/[controller]")]
[ApiController]
[Authorize]
public class EvenementController(IEvenementService evenementService, ILogger<EvenementController> logger) : ControllerBase
{

  #region Create

  [HttpPost]
  [Authorize(Roles = "Admin")]
  [EnableRateLimiting("RateLimitAdmin")]
  public async Task<ActionResult<EvenementDetailsResponseDto>> Create(EvenementRequestDto dto)
  {
    try
    {
      EvenementDetailsResponseDto result = await evenementService.CreateAsync(dto);
      logger.LogInformation("Nouvelle Categorie : {nom} avec l'id : {id} créée en DB",result.Nom, result.Id);
      return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
      
    }
    catch (Exception ex)
    {
      return BadRequest(new { message = ex.Message });
    }
  }

  #endregion

  #region GetLatest

  [HttpGet("latest")]
  [AllowAnonymous]
  [EnableRateLimiting("auth-limit")]
  public async Task<ActionResult<IEnumerable<EvenementShortResponseDto>>> GetLatest()
  {
    IEnumerable<EvenementShortResponseDto> result = await evenementService.GetLatestAsync();
    return Ok(result);
  }

  #endregion

  #region GetById

  [HttpGet("{id:guid}")]
  [AllowAnonymous]
  [EnableRateLimiting("auth-limit")]
  public async Task<ActionResult<EvenementDetailsResponseDto>> GetById(Guid id)
  {
    try
    {
      EvenementDetailsResponseDto result = await evenementService.GetByIdAsync(id);
      return Ok(result);
    }
    catch (KeyNotFoundException ex)
    {
      return NotFound(new { message = ex.Message });
    }
  }

  #endregion
}

