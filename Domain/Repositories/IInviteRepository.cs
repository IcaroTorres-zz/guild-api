using Domain.Entities;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface IInviteRepository
    {
        Task<bool> ExistsAsync(Expression<Func<Invite, bool>> predicate);
        Task<bool> ExistsWithIdAsync(Guid id);
        Task<Invite> GetByIdAsync(Guid id, bool readOnly = false);
        Task<Invite> GetForAcceptOperation(Guid id);
        IQueryable<Invite> Query(Expression<Func<Invite, bool>>? predicate = null, bool readOnly = false);
        Task<Invite> InsertAsync(Invite entity);
        Invite Remove(Invite entity);
    }
}