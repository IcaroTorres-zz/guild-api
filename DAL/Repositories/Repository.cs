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

		public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate, CancellationToken token = default)
		{
			return await Query(predicate, true).AnyAsync(token);
		}

		public async Task<bool> ExistsWithIdAsync(Guid id, CancellationToken token = default)
		{
			return await _dbSet.FindAsync(new []{ (object) id }, token) != null;
		}

		public async Task<T> GetByKeysAsync(CancellationToken token = default, params object[] keys)
		{
			return await _dbSet.FindAsync(keys, token);
		}

		public async Task<IReadOnlyList<T>> GetAllAsync(bool readOnly = false, CancellationToken token = default)
		{
			return await Query(e => true, readOnly).ToListAsync(cancellationToken: token);
		}

		public IQueryable<T> Query(Expression<Func<T, bool>>? predicate = null, bool readOnly = false)
		{
			var query = _dbSet
				.Where(e => !e.Disabled)
				.Where(predicate ?? (e => true));
			return readOnly ? query.AsNoTracking() : query;
		}

		public async Task<T> InsertAsync(T entity, CancellationToken token = default)
		{
			return (await _dbSet.AddAsync(entity, token)).Entity;
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