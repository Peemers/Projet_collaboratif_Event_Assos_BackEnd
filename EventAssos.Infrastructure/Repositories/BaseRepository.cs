using EventAssos.Core.Interfaces.Repositories;
using EventAssos.Infrastructure.DataBase.Context;
using Microsoft.EntityFrameworkCore;

namespace EventAssos.Infrastructure.Repositories;

public class BaseRepository<T, TId>(EventAssosDbContext context) : IBaseRepository<T, TId> where T : class
{
  private DbSet<T> _entities => context.Set<T>();

  public async Task<IEnumerable<T>> GetAllAsync()
  {
    return await _entities.ToListAsync();
  }

  public async Task<T?> GetByIdAsync(TId id)
  {
    return await _entities.FindAsync(id);
  }

  public async Task<T> AddAsync(T entity)
  {
    await _entities.AddAsync(entity);
    await context.SaveChangesAsync();
    return entity;
  }

  public async Task<T> UpdateAsync(T entity)
  {
    _entities.Update(entity);
    await context.SaveChangesAsync();
    return entity;
  }

  public async Task DeleteAsync(TId id)
  {
    T? entity = await _entities.FindAsync(id);
    if (entity != null)
    {
      _entities.Remove(entity);
      await context.SaveChangesAsync();
    }
  }
}