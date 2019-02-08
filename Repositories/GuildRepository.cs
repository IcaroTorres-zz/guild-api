
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
      
      if (!string.IsNullOrEmpty(master.GuildName)) return false;
      try
      { Add(new Guild
        { Name = guildName, MasterName = masterName, Members = new List<string>() { masterName } });
      } catch (Exception) { return false; }
      master.GuildName = guildName;
      master.IsGuildMaster = true;
      return true;
    }

    public bool AddMember(string guildName, string memberName)
    {
      var guild = Get(guildName);
      var member = GetUser(memberName);
      try
      {
        if ( (guild == null || member == null)
           || guild.Members.Any(m => m.Equals(memberName, StringComparison.OrdinalIgnoreCase))
           || ( !string.IsNullOrEmpty(member.GuildName) && !RemoveMember(memberName, guildName)) )
          return false;
        guild.Members.Add(memberName);
      } catch(Exception) { return false; }
      member.GuildName = guildName;
      return true;
    }
    public bool RemoveMember(string memberName, string guildName)
    {
      var guild = Get(guildName);
      var member = GetUser(memberName);
      try
      {
        if ( (guild == null || member == null)
           || ! guild.Members.Any(m => m.Equals(memberName, StringComparison.OrdinalIgnoreCase))
           || (member.Name.Equals(guild.MasterName, StringComparison.OrdinalIgnoreCase) && guild.Members.Count() > 1) )
          return false;

        guild.Members.Remove(memberName);
        if (guild.Members.Count == 0)
          Remove(guild);
      } catch(Exception) { return false; }
      member.GuildName = null;
      return true;
    }
    public bool TransferOwnership(string guildName, string userName)
    {
      var guild = Get(guildName);
      var user = GetUser(userName);
      if (user == null || guild == null || user.GuildName != guildName)
        return false;
      guild.MasterName = userName;
      user.IsGuildMaster = true;
      return true;
    }
    public IEnumerable<string> GetNthGuilds(int count = 20) => GetAll().Take(count).Select(g => g.Name);
    public new Guild Get (string guildName)
    {
      var guild =Context.Set<Guild> ().Find(guildName);
      if (guild != null) {
        guild.Members = LumenContext.Users
          .Where(u => !string.IsNullOrEmpty(u.GuildName)
            && u.GuildName.Equals(guild.Name, StringComparison.OrdinalIgnoreCase))
          .Select(u => u.Name).ToList();
      }
      return guild;
    }
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