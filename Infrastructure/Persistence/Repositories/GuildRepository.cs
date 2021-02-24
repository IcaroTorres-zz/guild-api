using Application.Common.Abstractions;
using Application.Common.Responses;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repositories
{
    public sealed class GuildRepository : IGuildRepository
    {
        private readonly IRepository<Guild> _baseRepository;

        public GuildRepository(IRepository<Guild> baseRepository)
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

        public async Task<Guild> GetByIdAsync(Guid id, bool readOnly = false, CancellationToken cancellationToken = default)
        {
            var entity = await _baseRepository.Query(readOnly: readOnly)
                .Include(x => x.Members)
                .SingleOrDefaultAsync(x => x.Id.Equals(id), cancellationToken);

            return entity ?? Guild.Null;
        }

        public async Task<Guild> GetForMemberHandlingAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var guild = await _baseRepository.Query(readOnly: false)
                .Include(x => x.Members)
                .ThenInclude(x => x.Memberships)
                .Include(x => x.Invites)
                .ThenInclude(x => x.Member)
                .SingleOrDefaultAsync(x => x.Id.Equals(id), cancellationToken);

            return guild ?? Guild.Null;
        }

        public async Task<PagedResponse<Guild>> PaginateAsync(int top = 20, int page = 1, CancellationToken cancellationToken = default)
        {
            var itemsQuery = _baseRepository.Query(readOnly: true).Include(x => x.Members);

            return await _baseRepository.PaginateAsync(itemsQuery, top, page, cancellationToken);
        }

        public async Task<Guild> InsertAsync(Guild model, CancellationToken cancellationToken = default)
        {
            return await _baseRepository.InsertAsync(model, cancellationToken);
        }

        public Guild Update(Guild model)
        {
            return _baseRepository.Update(model);
        }
    }
}