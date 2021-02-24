using Application.Common.Abstractions;
using Application.Common.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Win32.SafeHandles;
using System;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repositories
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

        public async Task<IApiResult> CommitAsync(IApiResult result, CancellationToken cancellationToken = default)
        {
            result = await SaveAsync(result, cancellationToken);
            if (result.Success) _contextTransaction?.CommitAsync(cancellationToken);
            HasOpenTransaction = false;
            return result;
        }

        public async Task<IApiResult> SaveAsync(IApiResult result, CancellationToken cancellationToken = default)
        {
            try
            {
                await _context.SaveChangesAsync(cancellationToken);
                return result;
            }
            catch (Exception exception)
            {
                await RollbackStatesAsync(cancellationToken);
                var errors = exception.ToApiError()
                                      .Prepend(new ApiError("database", "Data constraint violation. Register is invalid or already exists."))
                                      .ToArray();
                return result.SetExecutionError(HttpStatusCode.Conflict, errors);
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