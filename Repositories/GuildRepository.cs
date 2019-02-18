
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

    public Guild CreateGuild(string guildName, string masterName)
    {
      try
      {
        if (Get(guildName) != null)
          throw new ArgumentException($"error: Guild [{guildName}] already exist.");

        var master = GetUser(masterName);
        if (master != null)
        {
          if ( !string.IsNullOrEmpty(master.GuildId))
            throw new ArgumentException($"error: User [{master.Id}] already in guild [{guildName}].");
        }
        else
        {
          master = new User { GuildId = guildName, Id = masterName };
        }
        var newGuild = new Guild
        {
          Id = guildName,
          MasterId = master.Id,
          Members = new List<User>() { master }
        };
        Add(newGuild);
        return newGuild;
      } catch (Exception e) { throw e; }
    }

    public bool AddMember(string guildName, string memberName)
    {
      var guild = Get(guildName);
      var member = GetUser(memberName);
      try
      {
        if (guild == null)
          throw new ArgumentException($"error: target guild [{guildName}] not found.");
        if (member == null)
          throw new ArgumentException($"error: target user [{memberName}] not found.");
        if (guild.Members.Contains(member))
          throw new ArgumentException($"error: member [{memberName}] already in target [{guildName}] guild.");
        if (!string.IsNullOrEmpty(member.GuildId) && !RemoveMember(memberName, guildName))
          throw new ArgumentException($"error: member [{memberName}] is the Guildmaster of the other guild [{member.GuildId}].\n" +
                                      "Guildmasters can only leave guilds as the last remaining member.\n" +
                                      $"(You can try 'lumen.api/transfer/{member.GuildId}/[otherValidMemberName]/')");
        guild.Members.Add(member);
        Update(guild);
        member.GuildId = guild.Id;
        LumenContext.Users.Update(member);
        return true;
      } catch(Exception e) { throw e; }
    }
    public bool RemoveMember(string memberName, string guildName)
    {
      var guild = Get(guildName);
      var member = GetUser(memberName);
      try
      {
        if (guild == null)
          throw new ArgumentException($"error: target guild [{guildName}] not found.");
        if (member == null)
          throw new ArgumentException($"error: target user [{memberName}] not found.");
        if (!guild.Members.Contains(member))
          throw new ArgumentException($"error: member [{memberName}] not in target [{guildName}] guild.");
        if (member.Id.Equals(guild.MasterId, StringComparison.OrdinalIgnoreCase) && guild.Members.Count() > 1)
          throw new ArgumentException($"error: member [{memberName}] is the Guildmaster of target [{guildName}] guild.\n" +
                                      "Guildmasters can only leave guilds as the last remaining member.\n" +
                                      "(You can try 'lumen.api/transfer/{guildName}/[otherValidMemberName]/')");
                                      
        guild.Members.Remove(member);
        Update(guild);
        // LumenContext.Users.Update(member);
        if (!guild.Members.Any())
          Remove(guild);
        return true;
      } catch (Exception e) { throw e; }
    }
    public bool TransferOwnership(string guildName, string userName)
    {
      var guild = Get(guildName);
      var user = GetUser(userName);
      if (guild == null)
        throw new ArgumentException($"error: target guild [{guildName}] not found.");
      if (user == null)
        throw new ArgumentException($"error: target user [{userName}] not found.");
      if (user.GuildId != guildName)
        throw new ArgumentException($"error: target user [{userName}] already is in a different guild [{user.GuildId}], and can not become GuildMaster of [{guildName}] guild.");
      
      // var previousMaster = guild.Master;
      guild.MasterId = userName;
      Update(guild);
      return true;
    }
    public IQueryable<Guild> GetNthGuilds(int count = 20) => GetAll().Take(count);
    public User GetUser(string username) => LumenContext.Users.FirstOrDefault(u => u.Id.Equals(username, StringComparison.OrdinalIgnoreCase));
  }
}