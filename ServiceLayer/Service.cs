using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace api.Services {
    public class Service<TContext> : IService<TContext> where TContext : DbContext
    {
        protected readonly DbContext _context;
        public Service(DbContext context) => _context = context;

        // incremental methods
        public void Add<TEntity>(TEntity entity)
        where TEntity : class
        => _context.Set<TEntity>().Add(entity);

        public void AddRange<TEntity>(IEnumerable<TEntity> entities)
        where TEntity : class
        => _context.Set<TEntity>().AddRange(entities);

        public void Update<TEntity>(TEntity entity)
        where TEntity : class
        => _context.Set<TEntity>().Update(entity);

        public void UpdateRange<TEntity>(IEnumerable<TEntity> entities)
        where TEntity : class
        => _context.Set<TEntity>().UpdateRange(entities);


        // retrieval methods
        public TEntity Get<TEntity, TKey>(TKey key)
        where TEntity : class
        => _context.Set<TEntity>().Find(key);

        public IQueryable<TEntity> GetAll<TEntity>()
        where TEntity : class
        => _context.Set<TEntity>();

        public IQueryable<TEntity> Find<TEntity>(
            Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "")
            where TEntity : class
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();

            if(predicate != null)
                query = query.Where(predicate);

            foreach(var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                query = query.Include(includeProperty);

            return orderBy != null ? orderBy(query) : query;
        }
        // removal methods
        public void Remove<TEntity>(TEntity entity)
        where TEntity : class
        => _context.Set<TEntity>().Remove(entity);

        public void RemoveRange<TEntity>(IEnumerable<TEntity> entities)
        where TEntity : class
        => _context.Set<TEntity>().RemoveRange(entities);

        public void Complete() => _context.SaveChanges();
        public void Rollback() => _context.ChangeTracker.Entries().ToList().ForEach(x => x.Reload());
    }
}