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
	public sealed class MemberRepository : IMemberRepository
	{
		private readonly IRepository<Member> _baseRepository;

		public MemberRepository(IRepository<Member> baseRepository)
		{
			_baseRepository = baseRepository;
		}

		public async Task<bool> ExistsAsync(Expression<Func<Member, bool>> predicate, CancellationToken token = default)
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

		public async Task<Member> GetByIdAsync(Guid id, bool readOnly = false, CancellationToken token = default)
		{
			return await _baseRepository.GetByKeysAsync(token, id) ?? new NullMember();
		}

		public async Task<Member> GetByNameAsync(string name, bool readOnly = false, CancellationToken token = default)
		{
			var query = Query(x => x.Name.Equals(name));

			return await (readOnly ? query.AsNoTracking() : query).SingleOrDefaultAsync(token) ?? new NullMember();
		}

		public async Task<Member> GetForGuildOperationsAsync(Guid id, CancellationToken token = default)
		{
			return await _baseRepository.Query()
				.Include(x => x.Memberships)
				.Include(x => x.Guild)
				.ThenInclude(x => x.Members)
				.SingleOrDefaultAsync(x => x.Id.Equals(id), token) ?? new NullMember();
		}

		public IQueryable<Member> Query(Expression<Func<Member, bool>>? predicate = null, bool readOnly = false)
		{
			return _baseRepository.Query(predicate, readOnly);
		}

		public async Task<Member> InsertAsync(Member entity, CancellationToken token = default)
		{
			return await _baseRepository.InsertAsync(entity, token);
		}

		public Member Update(Member entity)
		{
			return _baseRepository.Update(entity);
		}

		public Member Remove(Member entity)
		{
			return _baseRepository.Remove(entity);
		}
	}
}