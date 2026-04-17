namespace EventAssos.Core.Interfaces.Repositories;

public interface IBaseRepository<T, TId> where T : class
{
  Task<IEnumerable<T>> GetAllAsync();
  Task<T?> GetByIdAsync(TId id);
  Task<T> AddAsync(T entity);
  Task<T> UpdateAsync(T entity);
  Task DeleteAsync(TId id);
}