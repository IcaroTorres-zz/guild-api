using System.Collections.Generic;
using lumen.api.Models;

namespace lumen.api.Repositories
{
  public interface IUserRepository : IRepository<User>
  {
    bool EnterTheGuild(string guildName, string userName);
    bool LeaveTheGuild(string userName, string guildName);
    Guild GetGuild(string guildName);
    Guild UserGuild(string userName);

    new User Get(string name);
  }
}