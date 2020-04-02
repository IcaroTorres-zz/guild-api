using Domain.Entities;
using Domain.Repositories;
using DAL.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class Repository<T> : IRepository<T> where T : EntityModel<T>
    {
        private readonly ApiContext Context;
        private readonly DbSet<T> DbSet;
        public Repository(ApiContext context)
        {
            Context = context;
            DbSet = Context.Set<T>();
        }

        public virtual async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
        {
            return await Query(predicate, true).AnyAsync();
        }

        public virtual async Task<bool> ExistsWithIdAsync(Guid id)
        {
            return await DbSet.FindAsync(id) != null;
        }

        public virtual async Task<T> GetByKeysAsync(params object[] keys)
        {
            return await DbSet.FindAsync(keys);
        }

        public virtual async Task<IReadOnlyList<T>> GetAllAsync(bool readOnly = false)
        {
            return await Query(e => true, readOnly).ToListAsync();
        }

        public virtual IQueryable<T> Query(Expression<Func<T, bool>>? predicate = null, bool readOnly = false)
        {
            var query = DbSet.Where(e => !e.Disabled).Where(predicate ?? (e => true));
            return readOnly ? query.AsNoTracking() : query;
        }

        public virtual async Task<T> InsertAsync(T entity)
        {
            return (await DbSet.AddAsync(entity)).Entity;
        }

        public virtual T Update(T entity)
        {
            return DbSet.Update(entity).Entity;
        }

        public virtual T Remove(T entity)
        {
            return DbSet.Remove(entity).Entity;
        }
    }
}