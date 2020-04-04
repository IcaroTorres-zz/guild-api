using System;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.Unities
{
	public interface IUnitOfWork : IDisposable
	{
		IUnitOfWork Begin();
		
		Task<int> CommitAsync(CancellationToken token = default);
		
		Task<int> SaveAsync(CancellationToken token = default);
		
		Task RollbackTransactionAsync(CancellationToken token = default);
		
		Task RollbackStatesAsync(CancellationToken token = default);
	}
}