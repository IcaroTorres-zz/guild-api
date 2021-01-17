using Domain.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.Unities
{
    public interface IUnitOfWork : IDisposable
	{
		public IGuildRepository Guilds { get; }
		public IMemberRepository Members { get; }
		public IInviteRepository Invites { get; }
		public IMembershipRepository Memberships { get; }
		bool HasOpenTransaction { get; }
		IUnitOfWork BeginTransaction();
		Task<int> CommitAsync(CancellationToken cancellationToken = default);
		Task<int> SaveAsync(CancellationToken cancellationToken = default);
		Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
		Task RollbackStatesAsync(CancellationToken cancellationToken = default);
	}
}
