using Domain.Repositories;
using Domain.Unities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Win32.SafeHandles;
using Persistence.Context;
using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace Persistence.Unities
{
    public sealed class UnitOfWork : IUnitOfWork
    {
        private readonly ApiContext _context;
        private IDbContextTransaction _contextTransaction;
        private bool Disposed { get; set; }
        private SafeHandle Handle { get; } = new SafeFileHandle(IntPtr.Zero, true);

        public UnitOfWork(
            ApiContext context,
            IGuildRepository guilds,
            IMemberRepository members,
            IInviteRepository invites,
            IMembershipRepository memberships)
        {
            _context = context;
            Guilds = guilds;
            Members = members;
            Invites = invites;
            Memberships = memberships;
        }

        public IGuildRepository Guilds { get; }
        public IMemberRepository Members { get; }
        public IInviteRepository Invites { get; }
        public IMembershipRepository Memberships { get; }
        public bool HasOpenTransaction { get; private set; } = false;

        public IUnitOfWork BeginTransaction()
        {
            _contextTransaction = _context.Database.BeginTransaction();
            HasOpenTransaction = true;
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
                var changes = await SaveAsync(cancellationToken);
                if (changes > 0) _contextTransaction?.CommitAsync(cancellationToken);
                HasOpenTransaction = false;
                return changes;
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
                return await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception exception)
            {
                await RollbackStatesAsync(cancellationToken);
                throw exception;
            }
        }

        public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
        {
            await RollbackStatesAsync(cancellationToken);
            if (_contextTransaction != null) await _contextTransaction?.RollbackAsync(cancellationToken);
            HasOpenTransaction = false;
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

            if (disposing)
            {
                Handle.Dispose();
            }
            _context.Dispose();
            _contextTransaction?.Dispose();

            _contextTransaction = null;
            HasOpenTransaction = false;
            Disposed = true;
        }
    }
}