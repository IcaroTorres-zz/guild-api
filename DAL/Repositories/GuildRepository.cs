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

		public async Task<bool> ExistsAsync(Expression<Func<Guild, bool>> predicate, CancellationToken token = default)
		{
			return await _baseRepository.ExistsAsync(predicate, token);
		}

		public async Task<bool> ExistsWithIdAsync(Guid id, CancellationToken token = default)
		{
			return await _baseRepository.ExistsWithIdAsync(id, token);
		}

		public async Task<bool> ExistsWithNameAsync(string name, CancellationToken token = default)
		{
			return await _baseRepository.ExistsAsync(x => x.Name.Equals(name), token);
		}

		public async Task<Guild> GetByIdAsync(Guid id, bool readOnly = false, CancellationToken token = default)
		{
			return await _baseRepository.GetByKeysAsync(token, id) ?? new NullGuild();
		}

		public async Task<Guild> GetForMemberHandlingAsync(Guid id, CancellationToken token = default)
		{
			return await _baseRepository.Query()
				.Include(x => x.Members)
				.ThenInclude(x => x.Memberships)
				.Include(x => x.Invites)
				.ThenInclude(x => x.Member)
				.SingleOrDefaultAsync(x => x.Id.Equals(id), token) ?? new NullGuild();
		}

		public async Task<IReadOnlyList<Guild>> GetAllAsync(bool readOnly = false, CancellationToken token = default)
		{
			return await Query(e => true, readOnly).ToListAsync(token);
		}

		public IQueryable<Guild> Query(Expression<Func<Guild, bool>>? predicate = null, bool readOnly = false)
		{
			return _baseRepository.Query(predicate, readOnly);
		}

		public async Task<Guild> InsertAsync(Guild entity, CancellationToken token = default)
		{
			return await _baseRepository.InsertAsync(entity, token);
		}

		public Guild Update(Guild entity)
		{
			return _baseRepository.Update(entity);
		}

		public Guild Remove(Guild entity)
		{
			return _baseRepository.Remove(entity);
		}
	}
}