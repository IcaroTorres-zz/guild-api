using Domain.Models;
using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface IInviteRepository
	{
		Task<Invite> GetForAcceptOperationAsync(Guid id, CancellationToken cancellationToken = default);
		Task<Invite> GetByIdAsync(Guid id, bool readOnly = false, CancellationToken cancellationToken = default);
		Task<Pagination<Invite>> PaginateAsync(Expression<Func<Invite, bool>> predicate = null, int top = 20, int page = 1, CancellationToken cancellationToken = default);
		Task<Invite> InsertAsync(Invite model, CancellationToken cancellationToken = default);
		Invite Update(Invite model);
	}
}