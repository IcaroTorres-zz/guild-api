using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Domain.Repositories
{
  public interface IRepository<T> where T : EntityModel<T>
  {
    Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
    Task<bool> ExistsWithIdAsync(Guid id);
    Task<T> GetByKeysAsync(params object[] keys);
    Task<IReadOnlyList<T>> GetAllAsync(bool readOnly = false);
    IQueryable<T> Query(Expression<Func<T, bool>>? predicate = null, bool readOnly = false);
    Task<T> InsertAsync(T entity);
    T Update(T entity);
    T Remove(T entity);
  }
}