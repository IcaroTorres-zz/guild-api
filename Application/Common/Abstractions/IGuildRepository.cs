using Application.Common.Responses;
using Domain.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Common.Abstractions
{
    public interface IGuildRepository
    {
        Task<bool> ExistsWithIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<bool> ExistsWithNameAsync(string name, CancellationToken cancellationToken = default);
        Task<bool> CanChangeNameAsync(Guid guid, string name, CancellationToken cancellationToken = default);
        Task<Guild> GetByIdAsync(Guid id, bool readOnly = false, CancellationToken cancellationToken = default);
        Task<Guild> GetForMemberHandlingAsync(Guid id, CancellationToken cancellationToken = default);
        Task<PagedResponse<Guild>> PaginateAsync(int top = 20, int page = 1, CancellationToken cancellationToken = default);
        Task<Guild> InsertAsync(Guild guild, CancellationToken cancellationToken = default);
        Guild Update(Guild guild);
    }
}