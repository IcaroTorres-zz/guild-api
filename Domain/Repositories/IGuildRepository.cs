using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface IGuildRepository
    {
        Task<bool> ExistsAsync(Expression<Func<Guild, bool>> predicate);
        Task<bool> ExistsWithIdAsync(Guid id);
        Task<bool> ExistsWithNameAsync(string name);
        Task<Guild> GetByIdAsync(Guid id, bool readOnly = false);
        Task<Guild> GetForMemberHandlingAsync(Guid id);
        Task<IReadOnlyList<Guild>> GetAllAsync(bool readOnly = false);
        IQueryable<Guild> Query(Expression<Func<Guild, bool>> predicate = null, bool readOnly = false);
        Task<Guild> InsertAsync(Guild guild);
        Guild Update(Guild guild);
        Guild Remove(Guild guild);
    }
}