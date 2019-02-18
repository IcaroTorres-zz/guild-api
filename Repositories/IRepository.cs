using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace lumen.api.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        // group of methods to get objects
        TEntity Get(string name);
        IQueryable<TEntity> GetAll();
        IQueryable<TEntity> Find(
            Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "");

        // group of methods to add objects
        void Add(TEntity entity);
        void AddRange(IEnumerable<TEntity> entities);
        // group of methods to add objects
        void Update(TEntity entity);
        void UpdateRange(IEnumerable<TEntity> entities);

        // group of methods to remove objects
        void Remove(TEntity entity);
        void RemoveRange(IEnumerable<TEntity> entities);
    }
}