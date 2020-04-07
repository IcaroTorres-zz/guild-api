using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using DAL.Context;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
	public sealed class Repository<T> : IRepository<T> where T : EntityModel<T>
	{
		private readonly DbSet<T> _dbSet;

		public Repository(ApiContext context)
		{
			_dbSet = context.Set<T>();
		}

		public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate,
			CancellationToken cancellationToken = default)
		{
			return await Query(predicate, true).AnyAsync(cancellationToken);
		}

		public async Task<bool> ExistsWithIdAsync(Guid id, CancellationToken cancellationToken = default)
		{
			return await _dbSet.FindAsync(new[] {(object) id}, cancellationToken) != null;
		}

		public async Task<T> GetByKeysAsync(CancellationToken cancellationToken = default, params object[] keys)
		{
			return await _dbSet.FindAsync(keys, cancellationToken);
		}

		public async Task<IReadOnlyList<T>> GetAllAsync(bool readOnly = false,
			CancellationToken cancellationToken = default)
		{
			return await Query(e => true, readOnly).ToListAsync(cancellationToken: cancellationToken);
		}

		public IQueryable<T> Query(Expression<Func<T, bool>> predicate = null, bool readOnly = false)
		{
			var query = _dbSet
				.Where(e => !e.Disabled)
				.Where(predicate ?? (e => true));
			return readOnly ? query.AsNoTracking() : query;
		}

		public async Task<T> InsertAsync(T entity, CancellationToken cancellationToken = default)
		{
			return (await _dbSet.AddAsync(entity, cancellationToken)).Entity;
		}

		public T Update(T entity)
		{
			return _dbSet.Update(entity).Entity;
		}

		public T Remove(T entity)
		{
			return _dbSet.Remove(entity).Entity;
		}
	}
}