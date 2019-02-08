
using System;
using System.Collections.Generic;
using System.Linq;
using lumen.api.Context;
using lumen.api.Models;

namespace lumen.api.Repositories
{
  public class GuildRepository : Repository<Guild>, IGuildRepository
  {
    public GuildRepository(LumenContext context) : base(context) { }
    public LumenContext LumenContext => Context as LumenContext;

    public bool AddMember(string guildName, string userName)
    {
      throw new NotImplementedException();
    }

    public bool CreateGuild(string guildName, string masterName)
    {
      throw new NotImplementedException();
    }

    public IEnumerable<string> GetGuilds(int count = 20)
    {
      throw new NotImplementedException();
    }

    public Dictionary<string, dynamic> GuildInfo(string guildName)
    {
      throw new NotImplementedException();
    }

    public bool RemoveMember(string userName, string guildName)
    {
      throw new NotImplementedException();
    }
  }
}