using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace api.Services
{
    public interface IService<TContext> where TContext : DbContext
    {
        // group of methods to get entities
        TEntity Get<TEntity, TKey>(TKey key) where TEntity : class;
        IQueryable<TEntity> GetAll<TEntity>() where TEntity : class;
        IQueryable<TEntity> Find<TEntity>(
            Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "") where TEntity : class;

        // group of methods to add entities
        void Add<TEntity>(TEntity entity) where TEntity : class;
        void AddRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class;
        // group of methods to add entities
        void Update<TEntity>(TEntity entity) where TEntity : class;
        void UpdateRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class;

        // group of methods to remove entities
        void Remove<TEntity>(TEntity entity) where TEntity : class;
        void RemoveRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class;
        void Complete();
        void Rollback();
    }
}