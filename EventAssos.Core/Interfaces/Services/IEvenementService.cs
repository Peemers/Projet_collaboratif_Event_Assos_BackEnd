using EventAssos.Core.DTOs.Request.EvenementRequestDtos;
using EventAssos.Core.DTOs.Response.EvenementResponseDtos;

namespace EventAssos.Core.Interfaces.Services;

public interface IEvenementService
{
  Task<EvenementDetailsResponseDto> CreateAsync(EvenementRequestDto dto);
  Task<IEnumerable<EvenementShortResponseDto>> GetLatestAsync();
  Task<EvenementDetailsResponseDto> GetByIdAsync(Guid id);
}