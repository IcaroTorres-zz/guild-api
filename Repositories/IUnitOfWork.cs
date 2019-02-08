using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lumen.api.Repositories
{
  public interface IUnitOfWork : IDisposable
  {
    IGuildRepository Guilds { get; }
    IUserRepository Users { get; }
    void Complete();
  }
}