using EventAssos.Core.DTOs.Request.CategoriesRequestDtos;
using EventAssos.Core.DTOs.Response.CategorieResponseDtos;
using EventAssos.Core.Interfaces.Services;
using EventAssos.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace EventAssos.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class CategorieController(
  ICategorieService categorieService,
  ILogger<CategorieController>logger) : ControllerBase
{
  #region GetAll

  [HttpGet(Name = "GetAll")]
  [EndpointSummary("Récupérer toutes les catégories")]
  [EndpointDescription("Retourne une liste simplifiée de tous les événements pour l'affichage en grille.")]
  [EnableRateLimiting("NormalRequest")]
  [AllowAnonymous]
  public async Task<ActionResult<IEnumerable<CategorieResponseDto>>> GetAll()
  {
    IEnumerable<CategorieResponseDto> result = await categorieService.GetAllAsync();
    return Ok(result);
  }

  #endregion

  #region GetById

  [HttpGet("{id}", Name = "GetById")]
  [EndpointSummary("Trouver un catégorie avec son id")]
  [EndpointDescription("Permet de trouver un catégorie en DB grace à son id")]
  [EnableRateLimiting("NormalRequest")]
  [AllowAnonymous]
  public async Task<ActionResult<CategorieResponseDto>> GetById(int id)
  {
    CategorieResponseDto categorie = await categorieService.GetByIdAsync(id);
    return Ok(categorie);
  }

  #endregion

  #region Create

  [HttpPost(Name =  "Create")]
  [Authorize(Roles = "Admin")]
  [EndpointSummary("Créer une nouvelles Catégorie")]
  [EndpointDescription("Permet à l'admin de créer une nouvelle catégorie")]
  [EnableRateLimiting("RateLimitAdmin")]
  public async Task<ActionResult<CategorieResponseDto>> Create(CategorieRequestDto dto)
  {
    try
    {
      CategorieResponseDto nouvelleCat = await categorieService.CreateAsync(dto);
      return CreatedAtAction(nameof(GetById), new { id = nouvelleCat.Id }, nouvelleCat);
    }
    catch (Exception ex)
    {
      return BadRequest(new { message = ex.Message });
    }
  }

  #endregion

  #region UpDate

  [HttpPut("{id}", Name = "Update")]
  [Authorize(Roles = "Admin")]
  [EndpointSummary("Changer le nom d'une catégorie")]
  [EndpointDescription("Permet à l'admin de modifier le nom d'une catégorie")]
  [EnableRateLimiting("RateLimitAdmin")]
  public async Task<IActionResult> Update(int id, CategorieRequestDto dto)
  {
    try
    {
      await categorieService.UpdateAsync(id, dto);
      return NoContent();
    }
    catch (Exception ex)
    {
      return BadRequest(new {message = ex.Message});
    }
  }

  #endregion

  #region Delete

  [HttpDelete("{id}", Name = "Delete")]
  [Authorize(Roles = "Admin")]
  [EndpointSummary("Supprimer une catégorie")]
  [EndpointDescription("Permet à l'admin de supprimer une catégorie")]
  [EnableRateLimiting("RateLimitAdmin")]
  public async Task<IActionResult> Delete(int id)
  {
    try
    {
      await categorieService.DeleteAsync(id);
      return NoContent();
    }
    catch (Exception ex)
    {
      return BadRequest(new {message = ex.Message});
    }
  }

  #endregion
}