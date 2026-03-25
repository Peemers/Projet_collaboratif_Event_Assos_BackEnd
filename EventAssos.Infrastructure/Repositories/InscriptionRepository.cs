using EventAssos.Core.Interfaces.Repositories;
using EventAssos.Domain.Entities;
using EventAssos.Infrastructure.DataBase.Context;
using Microsoft.EntityFrameworkCore;

namespace EventAssos.Infrastructure.Repositories;

public class InscriptionRepository(EventAssosDbContext context) : BaseRepository<Inscription, Guid>(context), IInscriptionRepository
{
  public async Task<Inscription?> GetInscriptionExisteAsync(Guid membreId, Guid evenementId)
  {
    Inscription? inscription = await context.Inscriptions
      .FirstOrDefaultAsync(i => i.MembreId == membreId && i.EvenementId == evenementId);
    
    return inscription;
  }
}