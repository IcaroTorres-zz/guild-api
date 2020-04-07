using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Entities.Nulls;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
	public sealed class GuildRepository : IGuildRepository
	{
		private readonly IRepository<Guild> _baseRepository;

		public GuildRepository(IRepository<Guild> baseRepository)
		{
			_baseRepository = baseRepository;
		}

		public async Task<bool> ExistsAsync(Expression<Func<Guild, bool>> predicate,
			CancellationToken cancellationToken = default)
		{
			return await _baseRepository.ExistsAsync(predicate, cancellationToken);
		}

		public async Task<bool> ExistsWithIdAsync(Guid id, CancellationToken cancellationToken = default)
		{
			return await _baseRepository.ExistsWithIdAsync(id, cancellationToken);
		}

		public async Task<bool> ExistsWithNameAsync(string name, CancellationToken cancellationToken = default)
		{
			return await _baseRepository.ExistsAsync(x => x.Name.Equals(name), cancellationToken);
		}

		public async Task<Guild> GetByIdAsync(Guid id, bool readOnly = false, CancellationToken cancellationToken = default)
		{
			return await _baseRepository.GetByKeysAsync(cancellationToken, id) ?? new NullGuild();
		}

		public async Task<Guild> GetForMemberHandlingAsync(Guid id, CancellationToken cancellationToken = default)
		{
			return await _baseRepository.Query()
				.Include(x => x.Members)
				.ThenInclude(x => x.Memberships)
				.Include(x => x.Invites)
				.ThenInclude(x => x.Member)
				.SingleOrDefaultAsync(x => x.Id.Equals(id), cancellationToken) ?? new NullGuild();
		}

		public async Task<IReadOnlyList<Guild>> GetAllAsync(bool readOnly = false,
			CancellationToken cancellationToken = default)
		{
			return await Query(e => true, readOnly).ToListAsync(cancellationToken);
		}

		public IQueryable<Guild> Query(Expression<Func<Guild, bool>> predicate = null, bool readOnly = false)
		{
			return _baseRepository.Query(predicate, readOnly);
		}

		public async Task<Guild> InsertAsync(Guild entity, CancellationToken cancellationToken = default)
		{
			return await _baseRepository.InsertAsync(entity, cancellationToken);
		}

		public Guild Update(Guild entity) => _baseRepository.Update(entity);

		public Guild Remove(Guild entity) => _baseRepository.Remove(entity);
	}
}