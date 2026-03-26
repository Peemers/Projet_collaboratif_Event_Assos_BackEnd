using EventAssos.Core.DTOs.Response.EvenementResponseDtos;

namespace EventAssos.Core.Interfaces.Services;

public interface IInscriptionService
{
  Task<EvenementDetailsResponseDto> InscrireMembreAsync(Guid evenementId, Guid membreId);
  
  Task AnnulerInscriptionAsync(Guid evenementId, Guid membreId);
}