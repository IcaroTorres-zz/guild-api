using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Common.Abstractions
{
    public interface IUnitOfWork : IDisposable
    {
        public IGuildRepository Guilds { get; }
        public IMemberRepository Members { get; }
        public IInviteRepository Invites { get; }
        public IMembershipRepository Memberships { get; }
        bool HasOpenTransaction { get; }
        IUnitOfWork BeginTransaction();
        Task<IApiResult> CommitAsync(IApiResult result, CancellationToken cancellationToken = default);
        Task<IApiResult> SaveAsync(IApiResult result, CancellationToken cancellationToken = default);
        Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
        Task RollbackStatesAsync(CancellationToken cancellationToken = default);
    }
}
