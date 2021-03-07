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
    public sealed class InviteRepository : IInviteRepository
    {
        private readonly IRepository<Invite> _baseRepository;

        public InviteRepository(IRepository<Invite> baseRepository)
        {
            _baseRepository = baseRepository;
        }

        public async Task<Invite> GetForAcceptOperationAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var model = await _baseRepository.Query(readOnly: false)
                .Include("guild.Members")
                .Include("member.guild.Members")
                .Include("member.Memberships")
                .SingleOrDefaultAsync(x => x.Id.Equals(id), cancellationToken);

            return model ?? Invite.Null;
        }

        public async Task<Invite> GetByIdAsync(Guid id, bool readOnly = false, CancellationToken cancellationToken = default)
        {
            var entity = await _baseRepository.Query(readOnly: readOnly)
                .Include("member")
                .Include("guild")
                .SingleOrDefaultAsync(x => x.Id.Equals(id), cancellationToken);

            return entity ?? Invite.Null;
        }

        public async Task<PagedResponse<Invite>> PaginateAsync(Expression<Func<Invite, bool>> predicate = null,
            int top = 20, int page = 1, CancellationToken cancellationToken = default)
        {
            var itemsQuery = _baseRepository.Query(predicate, readOnly: true)
                .Include("member")
                .Include("guild");

            return await _baseRepository.PaginateAsync(itemsQuery, top, page, cancellationToken);
        }

        public async Task<Invite> InsertAsync(Invite model, CancellationToken cancellationToken = default)
        {
            return await _baseRepository.InsertAsync(model, cancellationToken);
        }

        public Invite Update(Invite model)
        {
            return _baseRepository.Update(model);
        }
    }
}