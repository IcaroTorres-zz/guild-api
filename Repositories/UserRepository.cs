
using System;
using System.Collections.Generic;
using System.Linq;
using lumen.api.Context;
using lumen.api.Models;

namespace lumen.api.Repositories
{
  public class UserRepository : Repository<User>, IUserRepository
  {
    public UserRepository(LumenContext context) : base(context) { }
    public LumenContext LumenContext => Context as LumenContext;

    public bool EnterTheGuild(string guildName, string userName)
    {
      throw new NotImplementedException();
    }

    public bool LeaveTheGuild(string userName, string guildName)
    {
      throw new NotImplementedException();
    }
  }
}