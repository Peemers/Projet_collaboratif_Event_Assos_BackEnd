using EventAssos.Core.DTOs.Response.EvenementResponseDtos;
using EventAssos.Core.Interfaces.Repositories;
using EventAssos.Core.Interfaces.Services;
using EventAssos.Core.Mappers;
using EventAssos.Domain.Entities;
using EventAssos.Domain.Enums;
using Microsoft.Extensions.Logging;

namespace EventAssos.Core.Services;

public class InscriptionService(
  IInscriptionRepository inscriptionRepository, 
  IEvenementRepository evenementRepository, 
  IMembreRepository membreRepository,
  ILogger<InscriptionService> logger) : IInscriptionService
{
  #region InscrireMembre

  public async Task<EvenementDetailsResponseDto> InscrireMembreAsync(Guid evenementId, Guid membreId)
  {
    Evenement? evenement = await evenementRepository.GetAvecDetailsAsync(evenementId);
    Membre? membre = await membreRepository.GetByIdAsync(membreId);

    if (evenement == null) throw new KeyNotFoundException("L'événement demandé n'a pas été trouvé");
    if (membre == null) throw new KeyNotFoundException("Le membre demandé n'existe pas");
    
    Inscription? estExistant = await inscriptionRepository.GetInscriptionExisteAsync(membreId, evenementId);
    if (estExistant != null)
    {
      throw new Exception("Ce membre est deja inscrit à cet événement");
    }

    if (evenement.DateLimiteInscription < DateTime.UtcNow)
    {
      throw new Exception("Vous ne pouvez plus vous inscrire pour cet événement (date d'inscription dépassée)");
    }

    if (evenement.StatutEvenement == StatutEvenement.Annulé)
    {
      throw new Exception("Impossible de vous inscrire : événement annulé");
    }

    if (evenement.StatutEvenement == StatutEvenement.Terminé)
    {
      throw new Exception("Impossible de vous inscrire : événement deja terminé ");
    }
    
    int nombreInscrits = evenement.Inscriptions.Count(i => !i.EstEnAttente);
    bool estEnAttente = true;

    if (nombreInscrits < evenement.NbMax)
    {
      estEnAttente = false;
    }
    else if (evenement.ListeAttenteActive)
    {
      estEnAttente = true;
    }
    else
    {
      throw new Exception("L'événement est complet");
    }

    Inscription nouvelleInscription = new Inscription()
    {
      Id = Guid.NewGuid(),
      MembreId = membreId,
      EvenementId = evenementId,
      InscriptionDate = DateTime.UtcNow,
      EstEnAttente = estEnAttente,
    };
    
    await inscriptionRepository.AddAsync(nouvelleInscription);
    evenement.Inscriptions.Add(nouvelleInscription);

    return evenement.ToDetailsResponseDto();
  }

  #endregion

  #region AnnulerInscription

  public async Task AnnulerInscriptionAsync(Guid evenementId, Guid membreId)
  {
    Inscription? inscription = await inscriptionRepository.GetInscriptionExisteAsync(membreId, evenementId);
    if (inscription == null)
    {
      throw new KeyNotFoundException("Impossible d'annulé : inscription introuvable");
    }
    bool etaitEnAttente = inscription.EstEnAttente;
    Guid ASupprimer = inscription.Id;
    
    await inscriptionRepository.DeleteAsync(ASupprimer);
    logger.LogInformation("inscription {id} supprimée", ASupprimer);

    if (!etaitEnAttente)
    {
      Evenement? evenement = await evenementRepository.GetAvecDetailsAsync(evenementId);
      if (evenement != null)
      {
        Inscription? prochainAttente = evenement.Inscriptions
          .Where(i => i.EstEnAttente)
          .OrderBy(i => i.InscriptionDate)
          .FirstOrDefault();

        if (prochainAttente != null)
        {
          prochainAttente.EstEnAttente = false;
          await inscriptionRepository.UpdateAsync(prochainAttente);
          
          logger.LogInformation("Le membre {MembreId} est promu en liste principale pour l'événement {EvenementId}", prochainAttente.MembreId, evenement.Id);
        }
      }
    }
  }

  #endregion
}