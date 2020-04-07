using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Repositories
{
	public interface IMemberRepository
	{
		Task<bool> ExistsAsync(Expression<Func<Member, bool>> predicate, CancellationToken cancellationToken = default);
		Task<bool> ExistsWithIdAsync(Guid id, CancellationToken cancellationToken = default);
		Task<bool> ExistsWithNameAsync(string name, CancellationToken cancellationToken = default);
		Task<Member> GetByIdAsync(Guid id, bool readOnly = false, CancellationToken cancellationToken = default);
		Task<Member> GetByNameAsync(string name, bool readOnly = false, CancellationToken cancellationToken = default);
		Task<Member> GetForGuildOperationsAsync(Guid id, CancellationToken cancellationToken = default);
		IQueryable<Member> Query(Expression<Func<Member, bool>> predicate = null, bool readOnly = false);
		Task<Member> InsertAsync(Member entity, CancellationToken cancellationToken = default);
		Member Update(Member entity);
		Member Remove(Member entity);
	}
}