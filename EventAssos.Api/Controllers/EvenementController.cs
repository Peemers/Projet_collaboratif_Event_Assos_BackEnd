using EventAssos.Core.DTOs.Request.EvenementRequestDtos;
using EventAssos.Core.DTOs.Response.EvenementResponseDtos;
using EventAssos.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventAssos.Controllers;



[Route("api/[controller]")]
[ApiController]
[Authorize]
public class EvenementController(IEvenementService evenementService) : ControllerBase
{

  [HttpPost]
  [Authorize(Roles = "Admin")]
  public async Task<ActionResult<EvenementDetailsResponseDto>> Create(EvenementRequestDto dto)
  {
    try
    {
      EvenementDetailsResponseDto result = await evenementService.CreateAsync(dto);
      return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }
    catch (Exception ex)
    {
      return BadRequest(new { message = ex.Message });
    }
  }

  [HttpGet("latest")]
  [AllowAnonymous]
  public async Task<ActionResult<IEnumerable<EvenementShortResponseDto>>> GetLatest()
  {
    IEnumerable<EvenementShortResponseDto> result = await evenementService.GetLatestAsync();
    return Ok(result);
  }
  
  [HttpGet("{id:guid}")]
  [AllowAnonymous]
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
}

