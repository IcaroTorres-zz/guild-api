using System;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.Unities
{
	public interface IUnitOfWork : IDisposable
	{
		IUnitOfWork BeginTransaction();
		Task<int> CommitAsync(CancellationToken cancellationToken = default);
		Task<int> SaveAsync(CancellationToken cancellationToken = default);
		Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
		Task RollbackStatesAsync(CancellationToken cancellationToken = default);
	}
}