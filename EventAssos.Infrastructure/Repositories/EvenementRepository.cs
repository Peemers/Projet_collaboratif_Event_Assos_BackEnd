using EventAssos.Core.Interfaces.Repositories;
using EventAssos.Domain.Entities;
using EventAssos.Domain.Enums;
using EventAssos.Infrastructure.DataBase.Context;
using Microsoft.EntityFrameworkCore;

namespace EventAssos.Infrastructure.Repositories;

public class EvenementRepository(EventAssosDbContext context) : BaseRepository<Evenement, Guid>(context), IEvenementRepository
{
  public async Task<IEnumerable<Evenement>> GetFilterTenLatestAsync()
  {
    return await context.Evenements
      .Include(e => e.Categories)
      .Include(e => e.Inscriptions)
      .Where(e => e.StatutEvenement != StatutEvenement.Terminé && e.StatutEvenement == StatutEvenement.Annulé)
      .OrderByDescending(e => e.DateMaj)
      .Take(10)
      .ToListAsync();
  }

  public async Task<Evenement?> GetAvecDetailsAsync(Guid id)
  {
    return await context.Evenements
      .Include(e => e.Categories)
      .Include(e => e.Inscriptions)
      .ThenInclude(i => i.Membre)
      .FirstOrDefaultAsync(e => e.Id == id);
  }
}