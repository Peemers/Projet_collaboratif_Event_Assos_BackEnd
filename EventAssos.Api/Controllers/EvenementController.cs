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

  [HttpPost (Name =  "CreateEvenement")]
  [EndpointSummary("Créer un événement")]
  [EndpointDescription("Permet à l'admin de créer un événement")]
  [Authorize(Roles = "Admin")]
  [EnableRateLimiting("RateLimitAdmin")]
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

  #endregion

  #region Update

  [HttpPut("{id:guid}", Name = "UpdateEvenement")]
  [EndpointSummary("Mise à jour d'événement")]
  [EndpointDescription("Permet de modifier les informations des événements identifiés par id si celui-ci est encore au statut <EnAttente> ")]
  [Authorize(Roles = "Admin")]
  [EnableRateLimiting("RateLimitAdmin")]
  public async Task<IActionResult> Update(Guid id, EvenementRequestDto dto)
  {
    try
    {
      EvenementDetailsResponseDto result = await evenementService.UpdateAsync(id, dto);
      return Ok(result);
    }
    catch (KeyNotFoundException ex)
    {
      return NotFound(new { message = ex.Message });
    }
    catch (Exception ex)
    {
      return BadRequest(new { message = ex.Message });
    }
  }

  #endregion

  #region Delete

  [HttpDelete("{id:guid}", Name = "DeleteEvenement")]
  [EndpointSummary("Supprimer un événement")]
  [EndpointDescription("Permet de supprimer un événement si celui-ci est encore au statut <EnAttente>")]
  [Authorize(Roles = "Admin")]
  [EnableRateLimiting("RateLimitAdmin")]
  public async Task<IActionResult> Delete(Guid id)
  {
    try
    {
      await evenementService.DeleteAsync(id);
      return NoContent();
    }
    catch (KeyNotFoundException ex)
    {
      return NotFound(new { message = ex.Message });
    }
    catch (Exception ex)
    {
      return BadRequest(new { message = ex.Message });
    }
  }

  #endregion

  #region GetLatest

  [HttpGet("latest", Name = "GetLatest")]
  [EndpointSummary("Afficher les 10 derniers événements de la liste")]
  [EndpointDescription("Permet d'afficher les 10 derniers événements de la liste")]
  [AllowAnonymous]
  [EnableRateLimiting("auth-limit")]
  public async Task<ActionResult<IEnumerable<EvenementShortResponseDto>>> GetLatest()
  {
    IEnumerable<EvenementShortResponseDto> result = await evenementService.GetLatestAsync();
    return Ok(result);
  }

  #endregion

  #region Demarrer

  [HttpPatch("{id:guid}/demarrer", Name = "DemarrerEvenement")]
  [Authorize(Roles = "Admin")]
  [EndpointSummary("Démarrer un événement")]
  [EndpointDescription("Démarrer un événement tant que celui-ci est encore au statut <EnAttente>")]
  [EnableRateLimiting("RateLimitAdmin")]
  public async Task<IActionResult> Demarrer(Guid id)
  {
    try
    {
      await evenementService.DemarrerAsync(id);
      return NoContent();
    }
    catch (KeyNotFoundException ex)
    {
      return NotFound(new { message = ex.Message });
    }
    catch (Exception e)
    {
      return BadRequest(new { message = e.Message });
    }
  }

  #endregion

  #region Cloturer

  [HttpPatch("{id:guid}/cloturer", Name = "CloturerEvenement")]
  [Authorize(Roles = "Admin")]
  [EndpointSummary("Cloturer un événement")]
  [EndpointDescription("Cloturer un événement tant que celui-ci est encore au statut <EnCours>")]
  [EnableRateLimiting("RateLimitAdmin")]
  public async Task<IActionResult> Cloturer(Guid id)
  {
    try
    {
      await evenementService.CloturerAsync(id);
      return NoContent();
    }
    catch (KeyNotFoundException ex)
    {
      return NotFound(new { message = ex.Message });
    }
    catch (Exception ex)
    {
      return BadRequest(new { message = ex.Message });
    }
  }

  #endregion

  #region Annuler

  [HttpPatch("{id:guid}/annuler", Name = "AnnulerEvenement")]
  [Authorize(Roles = "Admin")]
  [EndpointSummary("Annuler un événement")]
  [EndpointDescription("Permet d'annuler un événement tant que celui-ci n'est pas <Terminé> ou deja <Annulé>")]
  [EnableRateLimiting("RateLimitAdmin")]
  public async Task<IActionResult> Annuler(Guid id)
  {
    try
    {
      await evenementService.AnnulerAsync(id);
      return NoContent();
    }
    catch (KeyNotFoundException ex)
    {
      return NotFound(new { message = ex.Message });
    }
    catch (Exception ex)
    {
      return BadRequest(new { message = ex.Message });
    }
  }

  #endregion

  #region GetSats

  [HttpGet(Name = "GetStats")]
  [AllowAnonymous]
  [EndpointSummary("Consulter les statistiques d'événement")]
  [EndpointDescription("Permet à tout le monde de consulter les statistiques des événements et donc de vérifier la viabilité")]
  [EnableRateLimiting("RateLimitAdmin")]
  public async Task<ActionResult<EvenementStatsResponseDto>> GetStats(Guid id)
  {
    try
    {
      EvenementStatsResponseDto result = await evenementService.GetStatsAsync(id);
      return Ok(result);
    }
    catch (KeyNotFoundException ex)
    {
      return NotFound(new { message = ex.Message });
    }
    catch (Exception ex)
    {
      return BadRequest(new { message = ex.Message });
    }
  }

  #endregion
  
  #region GetBlobalStats
  
  [HttpGet(Name = "GetGlobalStatsAsync")]
  [AllowAnonymous]
  [EndpointSummary("Consulter les statistique globales des événements")]
  [EndpointDescription("Permet à tout le monde de consulter les statistique globales des événements avec leur(s) catégorie(s) etc")]
  [EnableRateLimiting("NormalRequest")]

  

  public async Task<ActionResult<EvenementGlobalStatsResponseDto>> GetGlobalStatsAsync()
  {
    try
    {
      EvenementGlobalStatsResponseDto result = await evenementService.GetGlobalStatsAsync();
      return Ok(result);
    }
    catch (KeyNotFoundException ex)
    {
      throw new KeyNotFoundException(ex.Message);
    }
    catch (Exception e)
    {
      throw new Exception(e.Message);
    }
  }

  #endregion

  #region GetById

  [HttpGet("{id:guid}", Name =  "GetEvenementById")]
  [EndpointSummary("Trouver un événement avec son id")]
  [EndpointDescription("Permet de trouver un événement avec son Guid Id")]
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