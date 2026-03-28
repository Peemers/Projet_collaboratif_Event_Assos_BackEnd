using EventAssos.Core.DTOs.Request.CategoriesRequestDtos;
using EventAssos.Core.DTOs.Response.CategorieResponseDtos;
using EventAssos.Core.Interfaces.Services;
using EventAssos.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventAssos.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class CategorieController(
  ICategorieService categorieService,
  ILogger<CategorieController>logger) : ControllerBase
{
  #region GetAll

  [HttpGet]
  [AllowAnonymous]
  public async Task<ActionResult<IEnumerable<CategorieResponseDto>>> GetAll()
  {
    IEnumerable<CategorieResponseDto> result = await categorieService.GetAllAsync();
    return Ok(result);
  }

  #endregion

  #region GetById

  [HttpGet("{id}")]
  [AllowAnonymous]
  public async Task<ActionResult<CategorieResponseDto>> GetById(int id)
  {
    CategorieResponseDto categorie = await categorieService.GetByIdAsync(id);
    return Ok(categorie);
  }

  #endregion

  #region Create

  [HttpPost]
  [Authorize(Roles = "Admin")]
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

  [HttpPut("{id}")]
  [Authorize(Roles = "Admin")]
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

  [HttpDelete("{id}")]
  [Authorize(Roles = "Admin")]
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