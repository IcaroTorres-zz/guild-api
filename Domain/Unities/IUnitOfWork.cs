using System;

namespace Domain.Unities
{
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Begin a db transaction context
        /// </summary>
        IUnitOfWork Begin();

        /// <summary>
        /// Try committing all changes in transaction and perform Rollback if fail
        /// </summary>
        int Commit();

        /// <summary>
        /// Try saving a state of transaction and Rollback that changes if fail
        /// </summary>
        int Save();

        /// <summary>
        /// Discard all unsaved changes, dispatched when Commit fails and used when some part of a transaction fails
        /// </summary>
        void RollbackTransaction();

        /// <summary>
        /// Rollback all changes in the context instance
        /// </summary>
        void RollbackStates();
    }
}
