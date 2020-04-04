using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Repositories
{
	public interface IGuildRepository
	{
		Task<bool> ExistsAsync(Expression<Func<Guild, bool>> predicate, CancellationToken token = default);
		Task<bool> ExistsWithIdAsync(Guid id, CancellationToken token = default);
		Task<bool> ExistsWithNameAsync(string name, CancellationToken token = default);
		Task<Guild> GetByIdAsync(Guid id, bool readOnly = false, CancellationToken token = default);
		Task<Guild> GetForMemberHandlingAsync(Guid id, CancellationToken token = default);
		Task<IReadOnlyList<Guild>> GetAllAsync(bool readOnly = false, CancellationToken token = default);
		IQueryable<Guild> Query(Expression<Func<Guild, bool>> predicate = null, bool readOnly = false);
		Task<Guild> InsertAsync(Guild guild, CancellationToken token = default);
		Guild Update(Guild guild);
		Guild Remove(Guild guild);
	}
}