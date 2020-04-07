using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using DAL.Context;
using Domain.Unities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Win32.SafeHandles;

namespace DAL.Unities
{
	public sealed class UnitOfWork : IUnitOfWork
	{
		private readonly ApiContext _context;
		private int _changes;
		private IDbContextTransaction _contextTransaction;

		public UnitOfWork(ApiContext context)
		{
			_context = context;
		}

		private bool Disposed { get; set; }
		private SafeHandle Handle { get; } = new SafeFileHandle(IntPtr.Zero, true);

		public IUnitOfWork BeginTransaction()
		{
			_contextTransaction = _context.Database.BeginTransaction();
			return this;
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
		{
			try
			{
				await SaveAsync(cancellationToken);
				if (_changes > 0) _contextTransaction?.CommitAsync(cancellationToken);
				return _changes;
			}
			catch (Exception dbUpdateException)
			{
				await RollbackTransactionAsync(cancellationToken);
				throw new DbUpdateException(
					"Data constraint violation. Register is invalid or Already exists.", dbUpdateException);
			}
		}

		public async Task<int> SaveAsync(CancellationToken cancellationToken = default)
		{
			try
			{
				var newChanges = await _context.SaveChangesAsync(cancellationToken);
				_changes += newChanges;
				return newChanges;
			}
			catch (Exception exception)
			{
				await RollbackStatesAsync(cancellationToken);
				throw new DbUpdateException(
					"Data constraint violation. Register is invalid or Already exists.", exception);
			}
		}

		public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
		{
			await RollbackStatesAsync(cancellationToken);
			if (_contextTransaction != null) await _contextTransaction?.RollbackAsync(cancellationToken);
		}

		public async Task RollbackStatesAsync(CancellationToken cancellationToken = default)
		{
			await Task.Run(() => _context.ChangeTracker
				.Entries()
				.Where(e => e.State != EntityState.Added)
				.ToList()
				.ForEach(x => x.Reload()), cancellationToken);
		}

		private void Dispose(bool disposing)
		{
			if (Disposed) return;

			if (!disposing) return;
			Handle.Dispose();
			_contextTransaction?.Dispose();
			_context.Dispose();

			_contextTransaction = null;
			Disposed = true;
		}
	}
}