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
		Task<bool> ExistsAsync(Expression<Func<Guild, bool>> predicate, CancellationToken cancellationToken = default);
		Task<bool> ExistsWithIdAsync(Guid id, CancellationToken cancellationToken = default);
		Task<bool> ExistsWithNameAsync(string name, CancellationToken cancellationToken = default);
		Task<Guild> GetByIdAsync(Guid id, bool readOnly = false, CancellationToken cancellationToken = default);
		Task<Guild> GetForMemberHandlingAsync(Guid id, CancellationToken cancellationToken = default);
		Task<IReadOnlyList<Guild>> GetAllAsync(bool readOnly = false, CancellationToken cancellationToken = default);
		IQueryable<Guild> Query(Expression<Func<Guild, bool>> predicate = null, bool readOnly = false);
		Task<Guild> InsertAsync(Guild guild, CancellationToken cancellationToken = default);
		Guild Update(Guild guild);
		Guild Remove(Guild guild);
	}
}