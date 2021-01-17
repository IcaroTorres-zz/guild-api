using Domain.Models;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface IRepository<T> where T : EntityModel<T>
	{
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
        IQueryable<T> Query(Expression<Func<T, bool>> predicate = null, bool readOnly = false);
        Task<T> InsertAsync(T entity, CancellationToken cancellationToken = default);
        T Update(T entity);
        Task<Pagination<T>> PaginateAsync(IQueryable<T> query = null, int top = 20, int page = 1, CancellationToken cancellationToken = default);
    }
}