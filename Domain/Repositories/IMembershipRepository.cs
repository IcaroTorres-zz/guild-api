using Domain.Models;
using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface IMembershipRepository
	{
		Task<Pagination<Membership>> PaginateAsync(Expression<Func<Membership, bool>> predicate = null, int top = 20, int page = 1, CancellationToken cancellationToken = default);
        Task<Membership> InsertAsync(Membership model, CancellationToken cancellationToken = default);
        Membership Update(Membership model);
    }
}