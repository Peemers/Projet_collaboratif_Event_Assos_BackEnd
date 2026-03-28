using EventAssos.Core.DTOs.Request.EvenementRequestDtos;
using EventAssos.Core.DTOs.Response.EvenementResponseDtos;
using EventAssos.Core.Interfaces.Repositories;
using EventAssos.Core.Interfaces.Services;
using EventAssos.Core.Mappers;
using EventAssos.Domain.Entities;
using EventAssos.Domain.Enums;
using Microsoft.Extensions.Logging;

namespace EventAssos.Core.Services;

public class EvenementService(
  IEvenementRepository evenementRepository,
  ICategorieRepository categorieRepository,
  ILogger<EvenementService> logger) : IEvenementService
{
  #region CreateAsync

  public async Task<EvenementDetailsResponseDto> CreateAsync(EvenementRequestDto dto)
  {
    ValidationRegles(dto);
    Evenement nouvelEvenement = dto.ToEntity();

    foreach (int id in dto.CategorieIds)
    {
      Categorie? categorie = await categorieRepository.GetByIdAsync(id);
      if (categorie != null)
      {
        nouvelEvenement.Categories.Add(categorie);
      }
    }

    Evenement evenement = await evenementRepository.AddAsync(nouvelEvenement);

    logger.LogInformation("Événement {Nom} créé avec l'id : {Id}", evenement.Nom, evenement.Id);

    return evenement.ToDetailsResponseDto();
  }

  #endregion

  #region UpdateAsync

  public async Task<EvenementDetailsResponseDto> UpdateAsync(Guid id, EvenementRequestDto dto)
  {
    Evenement? evenement = await evenementRepository.GetAvecDetailsAsync(id);

    if (evenement == null) throw new KeyNotFoundException("L'événement demandé n'existe pas");
    if (evenement.StatutEvenement != StatutEvenement.EnAttente) throw new Exception("Seuls les événement tagués en attente peuvent etre modifiés");

    int nbInscrit = evenement.Inscriptions.Count(i => !i.EstEnAttente);
    if (dto.NbMax < nbInscrit)
    {
      throw new Exception($"Impossible de reduire le nombre max à {dto.NbMax} car il y a deja {nbInscrit} inscrits");
    }

    ValidationRegles(dto);
    evenement.UpdateEntity(dto);
    evenement.Categories.Clear();
    foreach (int catId in dto.CategorieIds)
    {
      Categorie? categorie = await categorieRepository.GetByIdAsync(catId);
      if (categorie == null) throw new KeyNotFoundException("Catégorie non trouvée");

      evenement.Categories.Add(categorie);
    }

    await evenementRepository.UpdateAsync(evenement);
    logger.LogInformation("Evénement {Id} mis à jour par l'admin", id);

    return evenement.ToDetailsResponseDto();
  }

  #endregion

  #region DeleteAsync

  public async Task DeleteAsync(Guid id)
  {
    Evenement? evenement = await evenementRepository.GetByIdAsync(id);
    if (evenement == null) throw new KeyNotFoundException("L'événement n'existe pas");
    if (evenement.StatutEvenement != StatutEvenement.EnAttente) throw new Exception("Suppression disponible uniquement sur les événements en attente");
    await evenementRepository.DeleteAsync(id);
    logger.LogInformation("L'événement {nom} avec l'id : {Id} a été supprimé", evenement.Nom, evenement.Id);
  }

  #endregion

  #region GetLatestAsync

  public async Task<IEnumerable<EvenementShortResponseDto>> GetLatestAsync()
  {
    IEnumerable<Evenement> evenements = await evenementRepository.GetFilterTenLatestAsync();

    return evenements.Select(e => e.ToShortResponseDto());
  }

  public async Task<EvenementDetailsResponseDto> GetByIdAsync(Guid id)
  {
    Evenement? result = await evenementRepository.GetAvecDetailsAsync(id);
    if (result == null)
    {
      throw new KeyNotFoundException("L'événement demandé n'existe pas");
    }

    return result.ToDetailsResponseDto();
  }

  #endregion
  
  

  #region Methode ValidationRegle

  private void ValidationRegles(EvenementRequestDto dto)
  {
    if (dto.NbMin > dto.NbMax)
    {
      throw new Exception("Le nombre de participants minimum ne peut pas dépasser le maximum");
    }

    if (dto.DateDebut <= DateTime.UtcNow)
      throw new Exception("L'événement doit commencer dans le futur");

    if (dto.DateFin <= dto.DateDebut)
      throw new Exception("La date de fin doit être postérieure à la date de début");

    if (dto.DateLimiteInscription > dto.DateDebut)
      throw new Exception("La date limite d'inscription ne peut pas être après la date de début de l'événement");
  }

  #endregion
}