using EventAssos.Core.DTOs.Request.EvenementRequestDtos;
using EventAssos.Core.DTOs.Response.EvenementResponseDtos;
using EventAssos.Core.Interfaces.Repositories;
using EventAssos.Core.Interfaces.Services;
using EventAssos.Core.Mappers;
using EventAssos.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace EventAssos.Core.Services;

public class EvenementService(
  IEvenementRepository evenementRepository,
  ICategorieRepository categorieRepository,
  ILogger<EvenementService> logger) : IEvenementService
{
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
    Evenement result = await evenementRepository.AddAsync(nouvelEvenement);
    
    logger.LogInformation("Événement {Nom} créé avec l'id : {Id}", result.Nom, result.Id);

    return result.ToDetailsResponseDto();
  }

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

  private void ValidationRegles(EvenementRequestDto dto)
  {
    if (dto.NbMin > dto.NbMax)
    {
      throw new Exception("Le nombre de participants minimum ne peut pas dépasser le maximum");
    }

    if (dto.DateDebut <= DateTime.UtcNow)
      throw new Exception("L'évenement doit commencer dans le futur");

    if (dto.DateFin <= dto.DateDebut)
      throw new Exception("La date de fin doit être postérieure à la date de début");

    if (dto.DateLimiteInscription > dto.DateDebut)
      throw new Exception("La date limite d'inscription ne peut pas être après la date de début de l'événement");
  }
}