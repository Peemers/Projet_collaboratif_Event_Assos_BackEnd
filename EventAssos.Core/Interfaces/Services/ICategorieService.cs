
using EventAssos.Core.DTOs.Request.CategoriesRequestDtos;
using EventAssos.Core.DTOs.Response.CategorieResponseDtos;
using EventAssos.Domain.Entities;

namespace EventAssos.Core.Interfaces.Services;

public interface ICategorieService
{
  Task<IEnumerable<CategorieResponseDto>> GetAllAsync();
  Task<CategorieResponseDto> GetByIdAsync(int id);
  Task<CategorieResponseDto> CreateAsync(CategorieRequestDto dto);
  Task UpdateAsync(int id, CategorieRequestDto dto);
  Task DeleteAsync(int id);
}