using Domain.Models;
using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface IMemberRepository
	{
		Task<bool> ExistsWithIdAsync(Guid id, CancellationToken cancellationToken = default);
		Task<bool> ExistsWithNameAsync(string name, CancellationToken cancellationToken = default);
		Task<bool> CanChangeNameAsync(Guid id, string name, CancellationToken cancellationToken = default);
		Task<bool> IsGuildMemberAsync(Guid id, Guid guildId, CancellationToken cancellationToken = default);
		Task<Member> GetByIdAsync(Guid id, bool readOnly = false, CancellationToken cancellationToken = default);
		Task<Member> GetForGuildOperationsAsync(Guid id, CancellationToken cancellationToken = default);
		Task<Pagination<Member>> PaginateAsync(Expression<Func<Member, bool>> predicate = null, int top = 20, int page = 1, CancellationToken cancellationToken = default);
		Task<Member> InsertAsync(Member model, CancellationToken cancellationToken = default);
		Member Update(Member model);
	}
}