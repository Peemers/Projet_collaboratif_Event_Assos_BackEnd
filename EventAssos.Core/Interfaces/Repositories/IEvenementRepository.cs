using EventAssos.Domain.Entities;

namespace EventAssos.Core.Interfaces.Repositories;

public interface IEvenementRepository : IBaseRepository<Evenement, Guid>
{
  Task<IEnumerable<Evenement>> GetFilterTenLatestAsync();
  
  Task<Evenement?> GetAvecDetailsAsync (Guid id);
}