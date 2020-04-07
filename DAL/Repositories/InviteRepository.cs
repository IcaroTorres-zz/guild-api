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

		public async Task<bool> ExistsAsync(Expression<Func<Invite, bool>> predicate,
			CancellationToken cancellationToken = default)
		{
			return await _baseRepository.ExistsAsync(predicate, cancellationToken);
		}

		public async Task<bool> ExistsWithIdAsync(Guid id, CancellationToken cancellationToken = default)
		{
			return await _baseRepository.ExistsWithIdAsync(id, cancellationToken);
		}

		public async Task<Invite> GetByIdAsync(Guid id, bool readOnly = false,
			CancellationToken cancellationToken = default)
		{
			return await _baseRepository.GetByKeysAsync(cancellationToken, id) ?? new NullInvite();
		}

		public async Task<Invite> GetForAcceptOperation(Guid id, CancellationToken cancellationToken = default)
		{
			return await Query()
				.Include(x => x.Guild)
				.ThenInclude(g => g.Members)
				.Include(x => x.Member)
				.ThenInclude(m => m.Memberships)
				.SingleOrDefaultAsync(x => x.Id.Equals(id), cancellationToken) ?? new NullInvite();
		}

		public IQueryable<Invite> Query(Expression<Func<Invite, bool>> predicate = null, bool readOnly = false)
		{
			return _baseRepository.Query(predicate, readOnly);
		}

		public async Task<Invite> InsertAsync(Invite entity, CancellationToken cancellationToken = default)
		{
			return await _baseRepository.InsertAsync(entity, cancellationToken);
		}

		public Invite Remove(Invite entity)
		{
			return _baseRepository.Remove(entity);
		}
	}
}