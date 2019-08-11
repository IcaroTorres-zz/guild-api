using Guild.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Guild.Services
{
    public interface IService<TContext> where TContext : DbContext
    {
        #region getters
        TEntity Get<TEntity>(Guid key, string includes = "", bool isreadonly = false) where TEntity : Entity<Guid>;
        IQueryable<TEntity> GetAll<TEntity>(bool isreadonly = false) where TEntity : Entity<Guid>;
        IQueryable<TEntity> GetAll<TEntity>(string includes, bool isreadonly = false) where TEntity : Entity<Guid>;
        IQueryable<TEntity> Find<TEntity>(Expression<Func<TEntity, bool>> predicate = null,
                                          Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderExpression = null,
                                          int? skip = null,
                                          int? top = null,
                                          string includes = "",
                                          bool isreadonly = false) where TEntity : Entity<Guid>;
        #endregion

        #region getters generic TKey
        TEntity Get<TEntity, TKey>(TKey key, string includes = "", bool isreadonly = false) where TEntity : Entity<TKey>;
        IQueryable<TEntity> GetAll<TEntity, TKey>(bool isreadonly = false) where TEntity : Entity<TKey>;
        IQueryable<TEntity> GetAll<TEntity, TKey>(string includes, bool isreadonly = false) where TEntity : Entity<TKey>;
        IQueryable<TEntity> Find<TEntity, TKey>(Expression<Func<TEntity, bool>> predicate = null,
                                                Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderExpression = null,
                                                int? skip = null,
                                                int? top = null,
                                                string includes = "",
                                                bool isreadonly = false) where TEntity : Entity<TKey>;
        #endregion

        #region setters
        TEntity Add<TEntity>(TEntity entity) where TEntity : Entity<Guid>;
        IEnumerable<TEntity> AddRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : Entity<Guid>;
        TEntity Update<TEntity>(TEntity entity) where TEntity : Entity<Guid>;
        IEnumerable<TEntity> UpdateRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : Entity<Guid>;
        #endregion

        #region setters generic TKey
        TEntity Add<TEntity, TKey>(TEntity entity) where TEntity : Entity<TKey>;
        IEnumerable<TEntity> AddRange<TEntity, TKey>(IEnumerable<TEntity> entities) where TEntity : Entity<TKey>;
        TEntity Update<TEntity, TKey>(TEntity entity) where TEntity : Entity<TKey>;
        IEnumerable<TEntity> UpdateRange<TEntity, TKey>(IEnumerable<TEntity> entities) where TEntity : Entity<TKey>;
        #endregion

        #region removals
        TEntity Remove<TEntity>(TEntity entity) where TEntity : Entity<Guid>;
        TEntity Remove<TEntity>(Guid key) where TEntity : Entity<Guid>;
        IEnumerable<TEntity> RemoveRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : Entity<Guid>;
        IEnumerable<TEntity> RemoveRange<TEntity>(IEnumerable<Guid> keys) where TEntity : Entity<Guid>;
        #endregion

        #region removals generic TKey
        TEntity Remove<TEntity, TKey>(TEntity entity) where TEntity : Entity<TKey>;
        TEntity Remove<TEntity, TKey>(TKey key) where TEntity : Entity<TKey>;
        IEnumerable<TEntity> RemoveRange<TEntity, TKey>(IEnumerable<TEntity> entities) where TEntity : Entity<TKey>;
        IEnumerable<TEntity> RemoveRange<TEntity, TKey>(IEnumerable<TKey> keys) where TEntity : Entity<TKey>;
        #endregion

        #region finishers
        int Commit();
        void Rollback();
        #endregion
    }
}