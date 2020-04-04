using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Repositories
{
	public interface IRepository<T> where T : EntityModel<T>
	{
		Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate, CancellationToken token = default);
		Task<bool> ExistsWithIdAsync(Guid id, CancellationToken token = default);
		Task<T> GetByKeysAsync(CancellationToken token = default, params object[] keys);
		Task<IReadOnlyList<T>> GetAllAsync(bool readOnly = false, CancellationToken token = default);
		IQueryable<T> Query(Expression<Func<T, bool>>? predicate = null, bool readOnly = false);
		Task<T> InsertAsync(T entity, CancellationToken token = default);
		T Update(T entity);
		T Remove(T entity);
	}
}