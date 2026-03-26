using EventAssos.Core.DTOs.Request.EvenementRequestDtos;
using EventAssos.Core.DTOs.Response.EvenementResponseDtos;
using EventAssos.Domain.Entities;
using EventAssos.Domain.Enums;

namespace EventAssos.Core.Mappers;

public static class EvenementMapper
{
  public static Evenement ToEntity(this EvenementRequestDto dto)
  {
    return new Evenement()
    {
      Id = Guid.NewGuid(),
      Nom = dto.Nom,
      Description = dto.Description,
      Lieu = dto.Lieu,
      DateDebut = dto.DateDebut,
      DateFin = dto.DateFin,
      NbMin = dto.NbMin,
      NbMax = dto.NbMax,
      ListeAttenteActive = dto.ListeAttenteActive,
      DateLimiteInscription = dto.DateLimiteInscription,
      StatutEvenement = StatutEvenement.EnAttente, //pour l'énoncé 
      DateDeCreation = DateTime.UtcNow,
      DateMaj = DateTime.UtcNow
    };
  }

  public static EvenementShortResponseDto ToShortResponseDto(this Evenement evenement)
  {
    return new EvenementShortResponseDto()
    {
      Id = evenement.Id,
      Nom = evenement.Nom,
      Description = evenement.Description.Length > 150 //ternaire si evenement.... > 150, énoncé
        ? evenement.Description.Substring(0, 150) /*[..150]*/ + "..."
        : evenement.Description,
      Lieu = evenement.Lieu,
      DateDebut = evenement.DateDebut,
      DateFin = evenement.DateFin,
      NbInscrits = evenement.Inscriptions.Count(i => !i.EstEnAttente), //calcul de la prop, exclure ceux en liste d'attente
      NbMin = evenement.NbMin,
      NbMax = evenement.NbMax,
      Categories = evenement.Categories.Select(c => c.Nom).ToList(),
      Statut = evenement.StatutEvenement.ToString(), //tostring, la prop dans le dto est une string}
      DateLimiteInscription = evenement.DateLimiteInscription,
      ListeAttenteActive = evenement.ListeAttenteActive,
    };
  }

  public static EvenementDetailsResponseDto ToDetailsResponseDto(this Evenement evenement)
  {
    return new EvenementDetailsResponseDto()
    {
      Id = evenement.Id,
      Nom = evenement.Nom,
      Description = evenement.Description,
      Lieu = evenement.Lieu,
      DateDebut = evenement.DateDebut,
      DateFin = evenement.DateFin,
      NbMin = evenement.NbMin,
      NbMax = evenement.NbMax,
      Statut = evenement.StatutEvenement.ToString(),
      Categories = evenement.Categories.Select(c => c.Nom).ToList(),
      ListeAttenteActive = evenement.ListeAttenteActive,
      DateLimiteInscription = evenement.DateLimiteInscription,

      MembresInscrits = evenement.Inscriptions
        .Where(i => !i.EstEnAttente)
        .OrderBy(i => i.InscriptionDate)
        .Select(i => i.Membre.Pseudo)
          .ToList(),
      
      ListeAttente = evenement.Inscriptions
        .Where(i => i.EstEnAttente)
        .OrderBy(i => i.InscriptionDate)
        .Select(i => i.Membre.Pseudo)
          .ToList(),
    };
  }
}