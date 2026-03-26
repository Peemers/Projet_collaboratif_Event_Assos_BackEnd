using EventAssos.Domain.Entities;

namespace EventAssos.Core.Interfaces.Repositories;

public interface IInscriptionRepository : IBaseRepository<Inscription, Guid>
{
  Task<Inscription?> GetInscriptionExisteAsync(Guid membreId, Guid evenementId);
}