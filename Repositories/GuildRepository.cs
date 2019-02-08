
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using lumen.api.Context;
using lumen.api.Models;

namespace lumen.api.Repositories
{
  public class GuildRepository : Repository<Guild>, IGuildRepository
  {
    public GuildRepository(LumenContext context) : base(context) { }
    public LumenContext LumenContext => Context as LumenContext;

    public bool CreateGuild(string guildName, string masterName)
    {
      var guild = Get(guildName);
      var master = GetUser(masterName) ?? new User() { Name = masterName };
      
      if (master.Guild != null || guild != null) return false;
      try
      {
        Add(new Guild
        {
          Name = guildName,
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

    public bool AddMember(string guildName, string memberName)
    {
      var guild = Get(guildName);
      var member = GetUser(memberName);
      if (guild == null || member == null) return false;

      try
      {
        if ( guild.Members.Contains(member, new UserEqualityComparer()) )
          return false;
        if (member.Guild != null && !RemoveMember(memberName, guildName))
          return false;

        guild.Members.Add(member);
        return true;
      } catch(Exception e) {
        return false;
      }
    }
    public bool RemoveMember(string memberName, string guildName)
    {
      var guild = Get(guildName);
      var member = GetUser(memberName);
      if (guild == null || member == null) return false;
      try
      {
        var memberComparer = new UserEqualityComparer();

        if (! guild.Members.Contains(member, memberComparer) )
          return false;
        if (member.Name.Equals(guild.MasterName, StringComparison.OrdinalIgnoreCase))
          return false;

        guild.Members.Remove(member);

        if (guild.Members.Count == 0)
          Remove(guild);

        return true;
      } catch(Exception e) {
        return false;
      }
    }
    public bool TransferOwnership(string guildName, string userName)
    {
      var guild = Get(guildName);
      var user = GetUser(userName);
      if (user == null || guild == null || user.Guild != guild)
        return false;
      guild.Master = user;
      return true;
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

    public User GetUser(string username) => LumenContext.Users.FirstOrDefault(u => u.Name.Equals(username, StringComparison.OrdinalIgnoreCase));
  }
}