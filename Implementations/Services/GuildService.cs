using Abstractions.Entities;
using Abstractions.Services;
using Context;
using DTOs;
using Implementations.Entities;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Implementations.Services
{
    public class GuildService : BaseService, IGuildService
    {
        private readonly string includesToMember = $"{nameof(Member.Memberships)},{nameof(Member.Guild)}";
        private readonly string includesToGuild = nameof(Guild.Members);
        public GuildService(ApiContext context) : base(context) { }

        public IGuild Create(GuildDto payload)
        {
            if (!(Query<Guild>(p => p.Name.Equals(payload.Name)).SingleOrDefault() is Guild guild))
            {
                var master = Query<Member>(
                    predicate: m => m.Name.Equals(payload.MasterName), 
                    included: includesToMember).SingleOrDefault();

                return Insert(new Guild(payload.Name, master));
            }
            throw new Exception($"A Guild with given name '{payload.Name}' already exists.");
        }

        public IGuild Update(GuildDto payload, Guid id)
        {
            var guild = Get(id);
            guild.ChangeName(payload.Name);
            guild.UpdateMembers(
                Query<Member>(m => !m.Disabled, included: includesToMember)
                .Join(payload.MemberIds, m => m.Id, id => id, (member, _) => member));

            return guild;
        }

        public IGuild Patch(Guid id, JsonPatchDocument<Guild> payload)
        {
            var guild = Get(id) as Guild;

            payload.ApplyTo(guild);

            return guild;
        }

        public IGuild AddMember(Guid id, string memberName)
        {
            var (guild, member) = GetGuildAndMember(id, memberName);

            guild.Invite(member);

            return guild;
        }

        public IGuild RemoveMember(Guid id, string memberName)
        {
            var (guild, member) = GetGuildAndMember(id, memberName);

            guild.KickMember(member);

            return guild;
        }

        public IGuild ChangeGuildMaster(Guid id, string memberName)
        {
            var (guild, member) = GetGuildAndMember(id, memberName);

            guild.Promote(member);

            return guild;
        }

        public IReadOnlyList<IGuild> List(int count = 20)
        {
            return GetAll<Guild>(included: includesToGuild, readOnly: true).Take(count).ToList();
        }

        public IGuild Get(Guid id)
        {
            var keys = new object[] { id };
            var collections = new List<string> { includesToGuild };

            return GetWithKeys<Guild>(keys, collections: collections) ?? throw new KeyNotFoundException($"Target guild with id '{id}' not found.");
        }

        private (IGuild, IMember) GetGuildAndMember(Guid id, string memberName)
        {
            var guild = Get(id);

            var member = Query<Member>(
                predicate: u => u.Name.Equals(memberName),
                included: includesToMember).SingleOrDefault() ?? throw new KeyNotFoundException($"Target user '{memberName}' not found.");

            return (guild, member);
        }

        public IGuild Delete(Guid id)
        {
            return Remove(Get(id));
        }
    }
}