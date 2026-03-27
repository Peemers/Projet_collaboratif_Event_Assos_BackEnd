using EventAssos.Core.DTOs.Request.CategoriesRequestDtos;
using EventAssos.Core.DTOs.Response.CategorieResponseDtos;
using EventAssos.Core.Interfaces.Repositories;
using EventAssos.Core.Interfaces.Services;
using EventAssos.Core.Mappers;
using EventAssos.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace EventAssos.Core.Services;

public class CategorieService(ICategorieRepository categorieRepository, ILogger<CategorieService> logger) : ICategorieService
{
  public async Task<IEnumerable<CategorieResponseDto>> GetAllAsync()
  {
    IEnumerable<Categorie> categories = await categorieRepository.GetAllAsync();
    return categories.Select(c => c.ToResponseDto());
  }

  public async Task<CategorieResponseDto> GetByIdAsync(int id)
  {
    Categorie categorie = await categorieRepository.GetByIdAsync(id) ??  throw new KeyNotFoundException($"La categorie avec l'id : {id}, n'existe pas");
    return categorie.ToResponseDto();
  }

  public async Task<CategorieResponseDto> CreateAsync(CategorieRequestDto dto) // pas de doublon du nom
  {
    if (await categorieRepository.ExistByNameAsync(dto.Nom))
    {
      logger.LogInformation("Impossible de créer la categorie {nom} deja existante", dto.Nom);
      throw new Exception($"Une categorie avec le nom {dto.Nom} existe deja");
    }
    Categorie entity = dto.ToEntity();
    Categorie result = await categorieRepository.AddAsync(entity);
    
    logger.LogInformation("categorie {nom} créé avec l'id {id}", dto.Nom, result.Id);
    
    return result.ToResponseDto();
  }

  public async Task UpdateAsync(int id, CategorieRequestDto dto)
  {
    Categorie? categorie = await categorieRepository.GetByIdAsync(id);
    if (categorie == null)
    {
      logger.LogInformation("Mise à jour impossible : Categorie non trouvée");
      throw new KeyNotFoundException("Categorie non trouvée");
    }
    categorie.Nom = dto.Nom;
    await categorieRepository.UpdateAsync(categorie);
    logger.LogInformation("Impossible de mettre à jour la categorie {nom} n'existe pas ", dto.Nom);
  }

  public async Task DeleteAsync(int id)
  {
    Categorie? categorie = await categorieRepository.GetByIdAsync(id);
    if (categorie == null) throw new Exception("Suppression impossible : categorie n'existe pas");
    await categorieRepository.DeleteAsync(id);
    logger.LogWarning("Categorie {nom} supprimée", categorie.Nom);
    
  }
}