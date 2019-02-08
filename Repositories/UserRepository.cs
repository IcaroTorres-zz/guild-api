
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
      var user = Get(userName);
      var guild = GetGuild(guildName);
      if (guild == null || user == null || user.IsGuildMaster) return false;
      user.Guild = guild;
      return true;
    }
    public new User Get (string name) => Context.Set<User> ().Find (name);
    public bool LeaveTheGuild(string userName, string guildName)
    {
      var user = Get(userName);
      var guild = GetGuild(guildName);
      if (guild == null || user == null || user.Guild == null || user.IsGuildMaster) return false;
      user.Guild = null;
      return true;
    }
    
    public Guild GetGuild(string guildname) => LumenContext.Guilds.FirstOrDefault(g => g.Name.Equals(guildname, StringComparison.OrdinalIgnoreCase));
    public Guild UserGuild(string username) => LumenContext.Guilds.FirstOrDefault(g => g.Name.Equals(Get(username).GuildName, StringComparison.OrdinalIgnoreCase));
  }
}