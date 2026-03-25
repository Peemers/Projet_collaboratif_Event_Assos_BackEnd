using EventAssos.Domain.Entities;

namespace EventAssos.Core.Interfaces.Repositories;

public interface IMembreRepository : IBaseRepository<Membre, Guid>
{
  Task<Membre?> GetByEmailAsync(string email);
  
  Task<bool> EmailExistsAsync(string email); //email unique
  Task<bool> PseudoExistsAsync(string pseudo); //pseudo unique
}