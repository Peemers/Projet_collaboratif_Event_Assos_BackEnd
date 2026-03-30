using EventAssos.Domain.Entities;

namespace EventAssos.Core.Interfaces.Repositories;
//implementation pour continuer le service de evenement -> à continuer plus tard avec les methodes specifique de categorie
public interface ICategorieRepository : IBaseRepository<Categorie, int>
{
  Task<bool> ExistByNameAsync (string nom);
}