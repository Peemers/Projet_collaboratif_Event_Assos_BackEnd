using EventAssos.Core.Interfaces.Repositories;
using EventAssos.Domain.Entities;
using EventAssos.Infrastructure.DataBase.Context;

namespace EventAssos.Infrastructure.Repositories;

public class CategorieRepository(EventAssosDbContext context) : BaseRepository<Categorie, int>(context), ICategorieRepository
{
  
}