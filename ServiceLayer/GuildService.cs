using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using api.Context;
using api.DTOs;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Services
{
    public class GuildService : Service<ApiContext>, IGuildService
    {
        public GuildService(ApiContext context) : base (context){ }
        public ApiContext _ctx => _context as ApiContext;

        public Guild CreateGuild(GuildDto payload)
        {
            var guild = Get<Guild, string>(payload.Id);
            if (guild != null)
                throw new InvalidOperationException($"Guild '{payload.Id}' already exist.");

            var master = Get<User, string>(payload.MasterId) ?? new User { Id = payload.MasterId };

            if (master.Guild != null)
                throw new InvalidOperationException($"User '{master.Id}' already in the guild '{master.Guild.Id}'.");

            var newGuild = new Guild()
            {
                Id = payload.Id,
                MasterId = master.Id,
                Members = new HashSet<User> { master }
                    .Union(payload.Members?.Select(id => Get<User, string>(id) ?? new User { Id = id }).ToHashSet())
                    .ToList()
            };
            Add(newGuild);
            _ctx.SaveChanges();
            return newGuild;
        }
        public Guild UpdateGuild(GuildDto payload)
        {
            var guild = Get<Guild, string>(payload.Id);
            if (guild == null)
                throw new ArgumentNullException($"Guild '{payload.Id}' already exist.", nameof(guild));

            var master = Get<User, string>(payload.MasterId) ?? new User { Id = payload.MasterId };

            if (master.Guild != null)
                throw new InvalidOperationException($"User '{master.Id}' already in the guild '{master.Guild.Id}'.");

            var newGuild = new Guild()
            {
                Id = payload.Id,
                MasterId = master.Id,
                Members = new HashSet<User> { master }
                    .Union(payload.Members?.Select(id => Get<User, string>(id) ?? new User { Id = id }).ToHashSet())
                    .ToList()
            };
            Add(newGuild);
            _ctx.SaveChanges();
            return newGuild;
        }
        public bool AddMember(string id, string memberId)
        {
            var guild = Get<Guild, string>(id);
            if (guild == null)
                throw new ArgumentNullException($"Target guild '{id}' not found.", nameof(guild));

            var member = Get<User, string>(memberId) ?? new User() { Id = memberId };

            if (guild.Members.Contains(member))
                throw new DuplicateWaitObjectException(nameof(member), $"Member '{memberId}' already in target '{id}' guild.");

            if (member.Guild != null && !RemoveMember(memberId, id))
                throw new InvalidOperationException($"Member '{memberId}' is the Guildmaster of the other guild '{member.GuildId}'. " +
                                                    "Guildmasters can only leave guilds as the last remaining member. (You can try PATCH to " +
                                                    $"'api/guilds/{member.GuildId}' with data 'masterId = otherValidMemberId' to transfer guild ownership).");

            guild.Members.Add(member);
            if (guild.Members.Count() == 1)
            {
                guild.Master = member;
                guild.MasterId = member.Id;
            }
            Update(guild);
            return true;
        }
        public bool RemoveMember(string id, string memberId)
        {
            var guild = Get<Guild, string>(id);
            if (guild == null)
                throw new ArgumentNullException($"Target guild '{id}' not found", nameof(guild));

            var member = Get<User, string>(memberId);
            if (member == null)
                throw new ArgumentNullException($"Target user '{memberId}' not found", nameof(member));

            if (!guild.Members.Contains(member))
                throw new KeyNotFoundException($"Member '{memberId}' not in target '{id}' guild");

            if (member.IsGuildMaster && guild.Members.Count() > 1)
                throw new InvalidOperationException($"Member '{memberId}' is the Guildmaster of target '{id}' guild. " +
                                                    $"Guildmasters can only leave guilds as the last remaining member. (You can try PATCH 'api/guilds/{id}' " +
                                                    "with data 'masterId = otherValidMemberId' to transfer guild ownership).");

            guild.Members.Remove(member);
            Update(guild);
            if (!guild.Members.Any()) Remove(guild);
            return true;
        }
        public bool Transfer(string id, string masterId)
        {
            var guild = Get<Guild, string>(id);
            if (guild == null)
                throw new ArgumentNullException($"Target guild '{id}' not found", nameof(guild));

            var user = Get<User, string>(masterId);
            if (user == null)
                throw new ArgumentNullException($"Target user '{masterId}' not found", nameof(user));

            if (user.Guild != null && user.GuildId != id)
                throw new InvalidOperationException($"Target user '{masterId}' already is in a different guild '{user.GuildId}', " +
                                                    $"and can not become GuildMaster of '{id}] guild.");

            if (user.Guild == null) guild.Members.Add(user);

            guild.MasterId = masterId;
            Update(guild);
            return true;
        }
        public IQueryable<Guild> GetNthGuilds(int count = 20) => GetAll<Guild>().Take(count);
        public async Task ApplyPatchAsync<TEntity>(TEntity entityName, List<PatchDto> patchDtos) where TEntity : class
        {
            var nameValuePairProperties = patchDtos.ToDictionary(a => a.PropertyName, a => a.PropertyValue);

            var dbEntityEntry = _context.Entry(entityName);
            dbEntityEntry.CurrentValues.SetValues(nameValuePairProperties);
            dbEntityEntry.State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}