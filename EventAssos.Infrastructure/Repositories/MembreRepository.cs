using EventAssos.Core.Interfaces.Repositories;
using EventAssos.Domain.Entities;
using EventAssos.Infrastructure.DataBase.Context;
using Microsoft.EntityFrameworkCore;

namespace EventAssos.Infrastructure.Repositories;

public class MembreRepository(EventAssosDbContext context) : BaseRepository<Membre, Guid>(context), IMembreRepository
{
  public async Task<Membre?> GetByEmailAsync(string email)
  {
    return await Context.Membres
      .FirstOrDefaultAsync(m => m.Email == email);
  }

  public async Task<bool> EmailExistsAsync(string email)
  {
    return await Context.Membres.AnyAsync(m => m.Email == email);
  }

  public async Task<bool> PseudoExistsAsync(string pseudo)
  {
    return await Context.Membres.AnyAsync(m => m.Pseudo == pseudo);
  }
}