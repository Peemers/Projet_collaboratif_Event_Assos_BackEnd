using EventAssos.Core.Interfaces.Repositories;
using EventAssos.Domain.Entities;
using EventAssos.Infrastructure.DataBase.Context;
using Microsoft.EntityFrameworkCore;

namespace EventAssos.Infrastructure.Repositories;

public class CategorieRepository(EventAssosDbContext context) : BaseRepository<Categorie, int>(context), ICategorieRepository
{
  public async Task<bool> ExistByNameAsync(string nom)
  {
    return await Context.Categories
      .AnyAsync(c => c.Nom.ToLower() == nom.ToLower());
  }
}