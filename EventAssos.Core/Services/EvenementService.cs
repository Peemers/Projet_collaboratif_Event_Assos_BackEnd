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
      if (categorie == null)
      {
        throw new Exception("Événement non créé - Categorie invalide");
      }

      nouvelEvenement.Categories.Add(categorie);
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
  
  #endregion

  #region GetByIdAsync

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

  #region GetStatsAsync

  public async Task<EvenementStatsResponseDto> GetStatsAsync(Guid id)
  {
    Evenement? result = await evenementRepository.GetAvecDetailsAsync(id);
    if (result == null)
    {
      throw new KeyNotFoundException("L'événement demandé n'existe pas");
    }

    return result.ToStatsResponseDto();
  }

  #endregion

  #region DemarrerAsync

  public async Task DemarrerAsync(Guid id)
  {
    Evenement? evenement = await evenementRepository.GetAvecDetailsAsync(id);

    if (evenement == null) throw new KeyNotFoundException("L'événement est introuvable");

    if (evenement.StatutEvenement != StatutEvenement.EnAttente) throw new Exception("Seul un événement en 'attente' peut etre démarré");
    if (evenement.DateDebut > DateTime.UtcNow) throw new Exception($"L'événement ne peut pas démarrer avant le {evenement.DateDebut}");

    int nbInscrit = evenement.Inscriptions.Count(i => !i.EstEnAttente);
    if (nbInscrit < evenement.NbMin) throw new Exception($"Le nombre minimum de participants : {evenement.NbMin} n'est pas atteint. Actuellement : {nbInscrit}");

    evenement.StatutEvenement = StatutEvenement.EnCours;
    evenement.DateMaj = DateTime.UtcNow;
    await evenementRepository.UpdateAsync(evenement);
    logger.LogInformation("L'événement {id} est maintenant EN COURS", id);
  }

  #endregion

  #region ClorturerAsync

  public async Task CloturerAsync(Guid id)
  {
    Evenement? evenement = await evenementRepository.GetByIdAsync(id);
    if (evenement == null) throw new KeyNotFoundException("Impossible de cloturer un événement introuvable");
    if (evenement.StatutEvenement != StatutEvenement.EnCours) throw new Exception("Seul un événement en cours peut etre cloturé");

    evenement.StatutEvenement = StatutEvenement.Terminé;
    evenement.DateMaj = DateTime.UtcNow;

    await evenementRepository.UpdateAsync(evenement);
    logger.LogInformation("L'événement {id} est maintenant TERMINE", id);
  }

  #endregion

  #region AnnulerAsync

  public async Task AnnulerAsync(Guid id)
  {
    Evenement? evenement = await evenementRepository.GetByIdAsync(id);
    if (evenement == null) throw new KeyNotFoundException("Impossible d'annuler un événement qui n'existe pas");
    if (evenement.StatutEvenement == StatutEvenement.Annulé || evenement.StatutEvenement == StatutEvenement.Terminé)
    {
      throw new Exception("Impossible d'annuler un événement deja terminé ou annulé");
    }

    evenement.StatutEvenement = StatutEvenement.Annulé;
    evenement.DateMaj = DateTime.UtcNow;

    await evenementRepository.UpdateAsync(evenement);
    logger.LogInformation("L'événement {id} est maintenant ANNULé", id);
  }

  #endregion

  #region ValidationRegle

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