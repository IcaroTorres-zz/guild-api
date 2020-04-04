using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Repositories
{
	public interface IInviteRepository
	{
		Task<bool> ExistsAsync(Expression<Func<Invite, bool>> predicate, CancellationToken token = default);
		Task<bool> ExistsWithIdAsync(Guid id, CancellationToken token = default);
		Task<Invite> GetByIdAsync(Guid id, bool readOnly = false, CancellationToken token = default);
		Task<Invite> GetForAcceptOperation(Guid id, CancellationToken token = default);
		IQueryable<Invite> Query(Expression<Func<Invite, bool>>? predicate = null, bool readOnly = false);
		Task<Invite> InsertAsync(Invite entity, CancellationToken token = default);
		Invite Remove(Invite entity);
	}
}