using Domain.Enums;
using Domain.Models;
using Domain.Models.Nulls;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public sealed class InviteRepository : IInviteRepository
    {
        private readonly IRepository<Invite> _baseRepository;

        public InviteRepository(IRepository<Invite> baseRepository)
        {
            _baseRepository = baseRepository;
        }

        public Task<Invite> GetForAcceptOperationAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return Task.Run(() =>
            {
                var model = _baseRepository.Query(x => x.Id.Equals(id), readOnly: false)
                    .Include(x => x.Guild)
                    .ThenInclude(g => g.Members)
                    .Include(x => x.Member)
                    .ThenInclude(m => m.Memberships)
                    .SingleOrDefault();

                return model ?? Invite.Null;
            }, cancellationToken);
        }

        public Task<Invite> GetByIdAsync(Guid id, bool readOnly = false, CancellationToken cancellationToken = default)
        {
            return Task.Run(() =>
            {
                var entity = _baseRepository.Query(x => x.Id.Equals(id), readOnly)
                    .Include(x => x.Member)
                    .Include(x => x.Guild)
                    .SingleOrDefault();

                return entity ?? Invite.Null;
            }, cancellationToken);
        }

        public async Task<bool> IsPendingAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var invite = await GetByIdAsync(id, readOnly: true, cancellationToken);

            return !(invite is INullObject) && invite.Status == InviteStatuses.Pending;
        }

        public async Task<Pagination<Invite>> PaginateAsync(Expression<Func<Invite, bool>> predicate = null,
            int top = 20, int page = 1, CancellationToken cancellationToken = default)
        {
            var itemsQuery = _baseRepository.Query(predicate, readOnly: true).Include(x => x.Guild).Include(x => x.Member);

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