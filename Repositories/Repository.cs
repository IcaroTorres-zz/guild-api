using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace lumen.api.Repositories {
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class {
        protected readonly DbContext Context;
        public Repository (DbContext context) => Context = context;

        // incremental methods
        public void Add (TEntity entity) => Context.Set<TEntity> ().Add (entity);
        public void AddRange (IEnumerable<TEntity> entities) => Context.Set<TEntity> ().AddRange (entities);

        // retrieval methods
        public TEntity Get (int id) => Context.Set<TEntity> ().Find (id);
        public IEnumerable<TEntity> Find (
            Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "") {
            IQueryable<TEntity> query = Context.Set<TEntity> ();

            if (predicate != null)
                query = query.Where (predicate);

            foreach (var includeProperty in includeProperties.Split (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                query = query.Include (includeProperty);

            return orderBy != null ? orderBy(query).ToList() : query.ToList();
        }
        public IEnumerable<TEntity> GetAll () => Context.Set<TEntity> ().ToList ();

        // removal methods
        public void Remove (TEntity entity) => Context.Set<TEntity> ().Remove (entity);
        public void RemoveRange (IEnumerable<TEntity> entities) => Context.Set<TEntity> ().RemoveRange (entities);
    }
}