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

    if (evenement.StatutEvenement != StatutEvenement.EnAttente)
    {
      throw new Exception("Inscription possible uniquement sur les événement en attente");
    }

    int nombreInscrits = evenement.Inscriptions.Count(i => !i.EstEnAttente);
    
    if (nombreInscrits >= evenement.NbMax && !evenement.ListeAttenteActive)
    {
      throw new Exception("L'événement est complet");
    }
    bool estEnAttente = nombreInscrits >= evenement.NbMax;
    
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
    Evenement? evenement = await evenementRepository.GetAvecDetailsAsync(evenementId);

    if (evenement == null) throw new KeyNotFoundException("L'événement demandé n'existe pas");
    if (evenement.StatutEvenement != StatutEvenement.EnAttente) throw new Exception("La désinscription n'est possible que sur un événement en attente");

    Inscription? inscription = await inscriptionRepository.GetInscriptionExisteAsync(membreId, evenementId);
    if (inscription == null)
    {
      throw new KeyNotFoundException("Impossible d'annulé : inscription introuvable");
    }

    evenement.Inscriptions.Remove(inscription);
    bool etaitEnAttente = inscription.EstEnAttente;
    Guid aSupprimer = inscription.Id;

    await inscriptionRepository.DeleteAsync(aSupprimer);
    logger.LogInformation("inscription {id} supprimée", aSupprimer);

    if (!etaitEnAttente)
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
  #endregion
}



