
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Context;
using api.Models;
using Microsoft.AspNetCore.Http;

namespace api.Repositories
{
  public class GuildRepository : Repository<Guild>, IGuildRepository
  {
    public GuildRepository(ApiContext context) : base(context) { }
    public ApiContext ApiContext => Context as ApiContext;
    public User GetUser(string name) =>
      ApiContext.Users.FirstOrDefault(u => u.Id.Equals(name, StringComparison.InvariantCultureIgnoreCase));

    public Guild CreateGuild(string guildId, string masterId)
    {
      var guild = Get(guildId);
      if (guild != null)
        throw new InvalidOperationException($"Guild '{guildId}' already exist.");

      var master = GetUser(masterId) ?? new User { Id = masterId };

      if (master.Guild != null)
        throw new InvalidOperationException($"User '{master.Id}' already in the guild '{master.Guild.Id}'.");
      
      var newGuild = new Guild()
      {
        Id = guildId,
        MasterId = master.Id,
        Members = new List<User>() { master }
      };
      Add(newGuild);
      return newGuild;
    }
    public bool AddMember(string name, string memberId)
    {
      var guild = Get(name);
      if (guild == null)
        throw new ArgumentNullException($"Target guild '{name}' not found", nameof(guild));

      var member = GetUser(memberId) ?? new User() { Id = memberId };

      if (guild.Members.Contains(member))
        throw new DuplicateWaitObjectException(nameof(member), $"Member '{memberId}' already in target '{name}' guild");

      if (member.Guild != null && !RemoveMember(memberId, name))
        throw new InvalidOperationException($"Member '{memberId}' is the Guildmaster of the other guild '{member.GuildId}'. " +
                                            "Guildmasters can only leave guilds as the last remaining member. (You can try PATCH to " +
                                            $"'api/guilds/{member.GuildId}' with data 'masterId = otherValidMemberId' to transfer guild ownership)");

      guild.Members.Add(member);
      if (guild.Members.Count() == 1)
      {
        guild.Master = member;
        guild.MasterId = member.Id;
      }
      Update(guild);
      return true;
    }
    public bool RemoveMember( string name, string memberId)
    {
      var guild = Get(name);
      if (guild == null)
        throw new ArgumentNullException($"Target guild '{name}' not found", nameof(guild));

      var member = GetUser(memberId);
      if (member == null)
        throw new ArgumentNullException($"Target user '{memberId}' not found", nameof(member));

      if (!guild.Members.Contains(member))
        throw new KeyNotFoundException($"Member '{memberId}' not in target '{name}' guild");

      if (member.IsGuildMaster && guild.Members.Count() > 1)
        throw new InvalidOperationException($"Member '{memberId}' is the Guildmaster of target '{name}' guild. " +
                                            "Guildmasters can only leave guilds as the last remaining member. (You can try PATCH 'api/guilds/{name}' " +
                                            "with data 'masterId = otherValidMemberId' to transfer guild ownership)");
                                    
      guild.Members.Remove(member);
      Update(guild);
      if (!guild.Members.Any()) Remove(guild);
      return true;
    }
    public bool Transfer(string name, string masterId)
    {
      var guild = Get(name);
      if (guild == null)
        throw new ArgumentNullException($"Target guild '{name}' not found", nameof(guild));

      var user = GetUser(masterId);
      if (user == null)
        throw new ArgumentNullException($"Target user '{masterId}' not found", nameof(user));

      if (user.Guild != null && user.GuildId != name)
        throw new InvalidOperationException($"Target user '{masterId}' already is in a different guild '{user.GuildId}', " +
                                            "and can not become GuildMaster of '{name}] guild");
      
      if (user.Guild == null) guild.Members.Add(user);

      guild.MasterId = masterId;
      Update(guild);
      return true;
    }
    public IQueryable<Guild> GetNthGuilds(int count = 20) => GetAll().Take(count);
  }
}