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
		Task<bool> ExistsAsync(Expression<Func<Invite, bool>> predicate, CancellationToken cancellationToken = default);
		Task<bool> ExistsWithIdAsync(Guid id, CancellationToken cancellationToken = default);
		Task<Invite> GetByIdAsync(Guid id, bool readOnly = false, CancellationToken cancellationToken = default);
		Task<Invite> GetForAcceptOperation(Guid id, CancellationToken cancellationToken = default);
		IQueryable<Invite> Query(Expression<Func<Invite, bool>> predicate = null, bool readOnly = false);
		Task<Invite> InsertAsync(Invite entity, CancellationToken cancellationToken = default);
		Invite Remove(Invite entity);
	}
}