using System;

namespace api.Repositories
{
  public interface IUnitOfWork : IDisposable
  {
    IGuildRepository Guilds { get; }
    IUserRepository Users { get; }
    void Complete();
    void Rollback();
  }
}