using System.Linq;
using api.Context;

namespace api.Repositories
{
  public class UnitOfWork : IUnitOfWork {
    private readonly ApiContext _ApiContext;
    public IGuildRepository Guilds { get; private set; }
    public IUserRepository Users { get; private set; }
    public UnitOfWork (ApiContext ApiContext)
    {
        _ApiContext = ApiContext;
        Guilds = new GuildRepository (_ApiContext);
        Users = new UserRepository (_ApiContext);
    }
    public void Complete () => _ApiContext.SaveChanges ();
    public void Rollback() => _ApiContext.ChangeTracker.Entries().ToList().ForEach(x => x.Reload());
    public void Dispose () => _ApiContext.Dispose ();
  }
}