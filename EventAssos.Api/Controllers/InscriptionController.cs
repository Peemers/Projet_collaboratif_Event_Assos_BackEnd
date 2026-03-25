using EventAssos.Core.DTOs.Response.EvenementResponseDtos;
using EventAssos.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventAssos.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class InscriptionController(IInscriptionService inscriptionService, ILogger<InscriptionController> logger) : ControllerBase
{
  [HttpPost("{evenementId:guid}")]
  [Authorize(Roles = "Membre")]
  public async Task<ActionResult<EvenementDetailsResponseDto>> CreateInscription(Guid evenementId)
  {
    string? membreClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

    if (string.IsNullOrEmpty(membreClaim))
    {
      return Unauthorized("Utilisateur non identifié dans le jeton.");
    }

    try
    {
      Guid membreId = Guid.Parse(membreClaim);
      EvenementDetailsResponseDto result = await inscriptionService.InscrireMembreAsync(evenementId, membreId);
      return Ok(result);
    }
    catch (KeyNotFoundException ex)
    {
      logger.LogWarning(ex, "Evenement ou membre introuvable");
      return NotFound(ex.Message);
    }
    catch (Exception ex)
    {
      logger.LogError(ex, "Erreur lors de l'inscription (doublon, complet, date dépassée)");
      return BadRequest(ex.Message);
    }
  }

  [HttpDelete("{evenementId:guid}")]
  [Authorize(Roles = "Membre")]
  public async Task<IActionResult> DeleteInscription(Guid evenementId)
  {
    string? membreClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
    if (string.IsNullOrEmpty(membreClaim))
    {
      return Unauthorized("Utilisateur non identifié dans le token");
    }

    try
    {
      Guid membreId = Guid.Parse(membreClaim);
      await inscriptionService.AnnulerInscriptionAsync(evenementId, membreId);
      return NoContent();
    }
    catch (KeyNotFoundException ex)
    {
      logger.LogWarning(ex, "Inscription introuvable pour annulation");
      return NotFound(ex.Message);
    }
    catch (Exception ex)
    {
      logger.LogError(ex, "Erreur lors de l'annulation de l'inscription");
      return BadRequest(ex.Message); 
    }
  }
}