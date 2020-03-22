using DataAccess.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Win32.SafeHandles;
using System;
using System.Linq;
using System.Runtime.InteropServices;

namespace DataAccess.Unities
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApiContext Context;
        private IDbContextTransaction ContextTransaction;

        protected bool Disposed { get; set; } = false;
        protected SafeHandle Handle { get; } = new SafeFileHandle(IntPtr.Zero, true);

        /// <summary>
        /// Construtor de Unidade de trabalho, injetada com os Contextos.
        /// </summary>
        /// <param name="context"/>
        public UnitOfWork(ApiContext context)
        {
            Context = context;
        }

        /// <summary>
        /// Begin a db transaction context
        /// </summary>
        public IUnitOfWork Begin()
        {
            ContextTransaction = Context.Database.BeginTransaction();

            return this;
        }


        /// <summary>
        /// Dispose all unmanaged objects and the opened context
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        /// <summary>
        /// Dispose all unmanaged objects and the opened context
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (Disposed)
            {
                return;
            }

            if (disposing)
            {
                Handle.Dispose();
                ContextTransaction?.Dispose();
                Context.Dispose();

                ContextTransaction = null;
                Disposed = true;
            }
        }

        /// <summary>
        /// Try committing all changes in transaction and perform Rollback if fail
        /// </summary>
        public void Commit()
        {
            try
            {
                Save();

                if (ContextTransaction != null)
                {
                    ContextTransaction.GetDbTransaction().Commit();
                }
            }
            catch (Exception dbex)
            {
                RollbackTransaction();
                throw dbex;
            }
        }

        public void Save()
        {
            try { 
                var changes = Context.SaveChanges(); 
            }
            catch (Exception dbex)
            {
                RollbackStates();
                throw dbex;
            }
        }

        /// <summary>
        /// Discard all unsaved changes, dispatched when Commit fails and used when some part of a transaction fails
        /// </summary>
        public void RollbackTransaction()
        {
            RollbackStates();

            if (ContextTransaction != null)
            {
                ContextTransaction.GetDbTransaction().Rollback();
            }
        }

        /// <summary>
        /// Rollback all changes in the context instance
        /// </summary>
        public void RollbackStates()
        {
            Context.ChangeTracker
                .Entries()
                .Where(e => e.State != EntityState.Added)
                .ToList()
                .ForEach(x => x.Reload());
        }
    }
}
