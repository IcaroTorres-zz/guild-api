using Application.Common.Abstractions;
using Application.Common.Responses;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repositories
{
    public sealed class MemberRepository : IMemberRepository
    {
        private readonly IRepository<Member> _baseRepository;

        public MemberRepository(IRepository<Member> baseRepository)
        {
            _baseRepository = baseRepository;
        }

        public async Task<bool> ExistsWithIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _baseRepository.ExistsAsync(x => x.Id.Equals(id), cancellationToken);
        }

        public async Task<bool> ExistsWithNameAsync(string name, CancellationToken cancellationToken = default)
        {
            return await _baseRepository.ExistsAsync(x => x.Name.Equals(name), cancellationToken);
        }

        public async Task<bool> CanChangeNameAsync(Guid id, string name, CancellationToken cancellationToken = default)
        {
            var isNameAlreadyTaken = await _baseRepository.ExistsAsync(x => !x.Id.Equals(id) && x.Name.Equals(name), cancellationToken);

            return !isNameAlreadyTaken;
        }

        public async Task<bool> IsGuildMemberAsync(Guid id, Guid guildId, CancellationToken cancellationToken = default)
        {
            return await _baseRepository.ExistsAsync(x => x.Id.Equals(id) && guildId.Equals(x.GuildId), cancellationToken);
        }

        public async Task<Member> GetForGuildOperationsAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var entity = await _baseRepository.Query()
                .Include(x => x.Guild.Invites)
                .Include(x => x.Guild.Members)
                .Include(x => x.Memberships)
                .SingleOrDefaultAsync(x => x.Id.Equals(id), cancellationToken);

            return entity ?? Member.Null;
        }

        public async Task<Member> GetByIdAsync(Guid id, bool readOnly = false, CancellationToken cancellationToken = default)
        {
            var entity = await _baseRepository.Query(readOnly: readOnly)
                .Include(x => x.Guild)
                .SingleOrDefaultAsync(x => x.Id.Equals(id), cancellationToken);

            return entity ?? Member.Null;
        }

        public async Task<PagedResponse<Member>> PaginateAsync(Expression<Func<Member, bool>> predicate = null,
            int top = 20, int page = 1, CancellationToken cancellationToken = default)
        {
            var itemsQuery = _baseRepository.Query(predicate, readOnly: true).Include(x => x.Guild);

            return await _baseRepository.PaginateAsync(itemsQuery, top, page, cancellationToken);
        }

        public async Task<Member> InsertAsync(Member model, CancellationToken cancellationToken = default)
        {
            return await _baseRepository.InsertAsync(model, cancellationToken);
        }

        public Member Update(Member model)
        {
            return _baseRepository.Update(model);
        }
    }
}