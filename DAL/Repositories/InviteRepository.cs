using System;
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
	public sealed class InviteRepository : IInviteRepository
	{
		private readonly IRepository<Invite> _baseRepository;

		public InviteRepository(IRepository<Invite> baseRepository)
		{
			_baseRepository = baseRepository;
		}

		public async Task<bool> ExistsAsync(Expression<Func<Invite, bool>> predicate, CancellationToken token = default)
		{
			return await _baseRepository.ExistsAsync(predicate, token);
		}

		public async Task<bool> ExistsWithIdAsync(Guid id, CancellationToken token = default)
		{
			return await _baseRepository.ExistsWithIdAsync(id, token);
		}

		public async Task<Invite> GetByIdAsync(Guid id, bool readOnly = false, CancellationToken token = default)
		{
			return await _baseRepository.GetByKeysAsync(token, id) ?? new NullInvite();
		}

		public async Task<Invite> GetForAcceptOperation(Guid id, CancellationToken token = default)
		{
			return await Query()
				.Include(x => x.Guild)
				.ThenInclude(g => g.Members)
				.Include(x => x.Member)
				.ThenInclude(m => m.Memberships)
				.SingleOrDefaultAsync(x => x.Id.Equals(id), token) ?? new NullInvite();
		}

		public IQueryable<Invite> Query(Expression<Func<Invite, bool>>? predicate = null, bool readOnly = false)
		{
			return _baseRepository.Query(predicate, readOnly);
		}

		public async Task<Invite> InsertAsync(Invite entity, CancellationToken token = default)
		{
			return await _baseRepository.InsertAsync(entity, token);
		}

		public Invite Remove(Invite entity)
		{
			return _baseRepository.Remove(entity);
		}
	}
}
