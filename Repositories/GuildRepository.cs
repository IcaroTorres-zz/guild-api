
using System;
using System.Collections.Concurrent;
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

    public bool CreateGuild(string guildName, User master)
    {
      try
      {
        Add(new Guild
        {
          Name = guildName,
          MasterName = master.Name,
          Master = master,
          Members = new List<User>() { master }
        });
        return true;
      }
      catch (Exception e)
      {
        return false;
      }
    }

    public IEnumerable<string> GetNthGuilds(int count = 20) => GetAll().Take(count).Select(g => g.Name);

    public Dictionary<string, dynamic> GuildInfo(string guildName)
    {
      var info = new Dictionary<string, dynamic>(StringComparer.OrdinalIgnoreCase);
      var guild = Get(guildName);
      info["guild"] = guild != null
        ? new Dictionary<string, dynamic>() { { "name", guild.Name }, { "guildmaster", guild.MasterName }, { "members", guild.Members } }
        : info["erro"] = "guild not found.";
      return info;
    }

    public bool RemoveMember(string userName, string guildName)
    {
      throw new NotImplementedException();
    }
  }
}