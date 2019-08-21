using Context;
using DTOs;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Services
{
    public class GuildService : Service<ApiContext>, IGuildService
    {
        public GuildService(ApiContext context) : base(context) { }
        public ApiContext Ctx => _context as ApiContext;

        /// <summary>
        /// Create a new Guild using GuildDto
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        public Guild CreateGuild(GuildDto payload)
        {
            var guild = Find<Guild>(p => p.Name.Equals(payload.Name)).FirstOrDefault();
            if (guild != null)
                throw new InvalidOperationException($"Guild '{payload.Name}' already exist.");

            var master = Get<User>(payload.MasterId) ?? new User(payload.MasterName);

            if (master.Guild != null)
                throw new InvalidOperationException($"User '{master.Name}' already in the guild '{master.Guild.Id}'.");

            return Add(new Guild(payload.Name, master)
            {
                Members = Find<User>(u => payload.Members.Contains(u.Id)).ToList()
            }.AddMember(master));
        }

        public Guild UpdateGuild(GuildDto payload)
        {
            var guild = Get<Guild>(payload.Id);
            if (guild == null)
                throw new ArgumentNullException(nameof(guild), $"Guild '{payload.Name}' already exist.");

            var master = Get<User>(payload.MasterId) ?? new User(payload.MasterName);

            if (master.Guild != null)
                throw new InvalidOperationException($"User '{master.Name}' already in the guild '{master.Guild.Name}'.");

            return Add(new Guild(payload.Name, master)
            {
                Members = Find<User>(u => payload.Members.Contains(u.Id)).ToList()
            }.AddMember(master));
        }

        public Guild AddMember(Guid id, string memberName)
        {
            var guild = Get<Guild>(id);
            if (guild == null)
                throw new ArgumentNullException(nameof(guild), $"Target guild ith id '{id}' not found.");

            var member = Find<User>(u => u.Name == memberName).FirstOrDefault() ?? new User(memberName);

            if (guild.Members.Contains(member))
                throw new DuplicateWaitObjectException(nameof(member), $"Member '{member.Name}' already in target '{guild.Name}' guild.");

            if (member.Guild != null)
                member.Guild.RemoveMember(member);

            guild.Members.Add(member);
            if (guild.Members.Count() == 1)
                guild.ChangeMaster(member);

            return Update(guild);
        }
        public Guild RemoveMember(Guid id, string memberName)
        {
            var guild = Get<Guild>(id);
            if (guild == null)
                throw new ArgumentNullException(nameof(guild), $"Target guild with id '{id}' not found.");

            var member = Find<User>(u => u.Name == memberName).FirstOrDefault();
            if (member == null)
                throw new ArgumentNullException(nameof(member), $"Target user '{memberName}' not found.");

            if (!guild.Members.Contains(member))
                throw new KeyNotFoundException($"Member '{memberName}' not in target '{guild.Name}' guild.");

            guild.RemoveMember(member);
            return !guild.Members.Any() ? Remove(guild) : Update(guild);
        }
        public Guild Transfer(Guid id, string masterName)
        {
            var guild = Get<Guild>(id);
            if (guild == null)
                throw new ArgumentNullException(nameof(guild), $"Target guild ith id '{id}' not found.");

            var member = Find<User>(u => u.Name == masterName).FirstOrDefault();
            if (member == null)
                throw new ArgumentNullException(nameof(member), $"Target user '{masterName}' not found");

            if (member.Guild != null && member.GuildId != id)
                throw new InvalidOperationException($"Target user '{member.Name}' already is in a different guild '{member.Guild.Name}', " +
                                                    $"and cannot become GuildMaster of '{guild.Name}] guild.");

            if (member.Guild == null) guild.AddMember(member);
            return Update(guild.ChangeMaster(member));
        }
        public IQueryable<Guild> List(int count = 20) => GetAll<Guild>().Take(count);

        // deprecated
        public TEntity ApplyPatchAsync<TEntity>(TEntity entityName, List<PatchDto> patchDtos) where TEntity : Entity<Guid>
        {
            var nameValuePairProperties = patchDtos.ToDictionary(a => a.PropertyName, a => a.PropertyValue);

            var dbEntityEntry = _context.Entry(entityName);
            dbEntityEntry.CurrentValues.SetValues(nameValuePairProperties);
            return Update(dbEntityEntry.Entity);
        }
    }
}