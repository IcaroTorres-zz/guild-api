
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Context;
using api.Models;

namespace api.Repositories
{
  public class GuildRepository : Repository<Guild>, IGuildRepository
  {
    public GuildRepository(LumenContext context) : base(context) { }
    public LumenContext LumenContext => Context as LumenContext;

    public User GetUser(string name) =>
      LumenContext.Users
                  .FirstOrDefault(u => u.Name.Equals(name,
                                                     StringComparison.InvariantCultureIgnoreCase));
    public Guild CreateGuild(string guildName, string masterName)
    {
      if (Get(guildName) != null)
        throw new ArgumentException($"409||Guild '{guildName}' already exist.");

      var master = GetUser(masterName);
      if (master != null)
      {
        if (master.Guild != null)
          throw new ArgumentException($"409||User [{master.Name}] already in the guild [{master.Guild.Name}].");
      }
      else master = new User { Name = masterName };
      
      var newGuild = new Guild()
      {
        Name = guildName,
        MasterName = master.Name,
        Members = new List<User>() { master }
      };
      Add(newGuild);
      return newGuild;
    }
    public bool AddMember(string name, string memberName)
    {
      var guild = Get(name);
      if (guild == null)
        throw new ArgumentException($"404||Target guild [{name}] not found.");

      var member = GetUser(memberName) ?? new User() { Name = memberName };
      if (guild.Members.Contains(member))
        throw new ArgumentException($"409||Member [{memberName}] already in target [{name}] guild.");

      if (member.Guild != null && !RemoveMember(memberName, name))
        throw new ArgumentException($"409||Member [{memberName}] is the Guildmaster of the other guild [{member.GuildName}]. " +
                                    "Guildmasters can only leave guilds as the last remaining member. " +
                                    $"(You can try PATCH to 'api/guilds/{member.GuildName}' " +
                                    "with data { masterName = [otherValidMemberName] } to transfer guild ownership).");

      guild.Members.Add(member);
      if (guild.Members.Count() == 1)
      {
        guild.Master = member;
        guild.MasterName = member.Name;
      }
      Update(guild);
      // member.GuildName = guild.Name;
      // LumenContext.Users.Update(member);
      return true;
    }
    public bool RemoveMember( string name, string memberName)
    {
      var guild = Get(name);
      if (guild == null)
        throw new ArgumentException($"404||Target guild [{name}] not found.");

      var member = GetUser(memberName);
      if (member == null)
        throw new ArgumentException($"404||Target user [{memberName}] not found.");

      if (!guild.Members.Contains(member))
        throw new ArgumentException($"404||Member [{memberName}] not in target [{name}] guild.");

      if (member.IsGuildMaster && guild.Members.Count() > 1)
        throw new ArgumentException($"409||Member [{memberName}] is the Guildmaster of target [{name}] guild. " +
                                    "Guildmasters can only leave guilds as the last remaining member. " +
                                    $"(You can try PATCH 'api/guilds/{name}' " +
                                    "with data { masterName = [otherValidMemberName] } to transfer guild ownership).");
                                    
      guild.Members.Remove(member);
      Update(guild);
      // LumenContext.Users.Update(member);
      if (!guild.Members.Any()) Remove(guild);
      return true;
    }
    public bool TransferOwnership(string name, string masterName)
    {
      var guild = Get(name);
      if (guild == null)
        throw new ArgumentException($"404||Target guild [{name}] not found.");

      var user = GetUser(masterName);
      if (user == null)
        throw new ArgumentException($"404||Target user [{masterName}] not found.");

      if (user.Guild != null && user.GuildName != name)
        throw new ArgumentException($"409||Target user [{masterName}] already is in a different guild [{user.GuildName}], and can not become                                GuildMaster of [{name}] guild.");
      
      if (user.Guild == null) guild.Members.Add(user);

      guild.MasterName = masterName;
      Update(guild);
      return true;
    }
    public IQueryable<Guild> GetNthGuilds(int count = 20) => GetAll().Take(count);
  }
}