using EventAssos.Core.DTOs.Request.EvenementRequestDtos;
using EventAssos.Core.DTOs.Response.EvenementResponseDtos;

namespace EventAssos.Core.Interfaces.Services;

public interface IEvenementService
{
  Task<EvenementDetailsResponseDto> CreateAsync(EvenementRequestDto dto);
  Task<IEnumerable<EvenementShortResponseDto>> GetLatestAsync();
  Task<EvenementDetailsResponseDto> GetByIdAsync(Guid id);
  Task<EvenementDetailsResponseDto> UpdateAsync(Guid id, EvenementRequestDto dto);
  Task<EvenementStatsResponseDto> GetStatsAsync(Guid id);
  Task DeleteAsync(Guid id);
  Task DemarrerAsync(Guid id);
  Task CloturerAsync(Guid id);
  Task AnnulerAsync(Guid id);
  
  //todo Task<EvenementStatsResponseDto>GetStatsAsync
}