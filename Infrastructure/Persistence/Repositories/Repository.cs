using Application.Common.Abstractions;
using Application.Common.Responses;
using Domain.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repositories
{
    public sealed class Repository<T> : IRepository<T> where T : EntityModel<T>
    {
        private readonly DbSet<T> _dbSet;

        public Repository(ApiContext context)
        {
            _dbSet = context.Set<T>();
        }

        public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await Query(predicate, true).AnyAsync();
        }

        public IQueryable<T> Query(Expression<Func<T, bool>> predicate = null, bool readOnly = false)
        {
            var set = readOnly ? _dbSet.AsNoTracking() : _dbSet;
            return set.Where(predicate ?? (_ => true));
        }

        public async Task<T> InsertAsync(T entity, CancellationToken cancellationToken = default)
        {
            if (entity is INullObject) return entity;
            var entry = await _dbSet.AddAsync(entity, cancellationToken);

            return entry.Entity;
        }

        public T Update(T entity)
        {
            if (entity is INullObject) return entity;
            return _dbSet.Update(entity).Entity;
        }

        public async Task<PagedResponse<T>> PaginateAsync(IQueryable<T> query = null, int top = 20, int page = 1, CancellationToken cancellationToken = default)
        {
            var itemsQuery = query ?? Query(_ => true, true);
            var totalCount = itemsQuery.Count();
            var skip = (page - 1) * top;
            var items = await itemsQuery.Skip(skip).Take(top).ToListAsync(cancellationToken);

            return new PagedResponse<T>(items, totalCount, top, page);
        }
    }
}