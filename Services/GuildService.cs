using Context;
using DTOs;
using Entities;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Services
{
    public class GuildService : BaseService, IGuildService
    {
        public GuildService(ApiContext context) : base(context) { }

        public Guild CreateOrUpdate(GuildDto payload, Guid id = new Guid())
        {
            var master = Query<User>(u => u.Name.Equals(payload.MasterName)).SingleOrDefault() ?? new User(payload.MasterName);

            var guild = Query<Guild>(p => p.Name.Equals(payload.Name) || p.Id.Equals(id)).FirstOrDefault() ?? Insert(new Guild(payload.Name, master));

            return guild.PromoteToGuildMaster(master);
        }

        public Guild Update(Guid id, JsonPatchDocument<Guild> payload)
        {
            var guild = Get(id);

            payload.ApplyTo(guild);

            return guild;
        }

        public Guild AddMember(Guid id, string memberName)
        {
            var (guild, member) = GetGuildAndMamber(id, memberName);

            return guild.InviteToGuild(member);
        }

        public Guild RemoveMember(Guid id, string memberName)
        {
            var (guild, member) = GetGuildAndMamber(id, memberName);

            return guild.KickMember(member);
        }

        public Guild ChangeGuildMaster(Guid id, string memberName)
        {
            var (guild, member) = GetGuildAndMamber(id, memberName);

            return guild.PromoteToGuildMaster(member);
        }

        public IReadOnlyList<Guild> List(int count = 20)
        {
            return GetAll<Guild>().Take(count).ToList();
        }

        public Guild Get(Guid id)
        {
            return GetWithKeys<Guild>(id) ?? throw new KeyNotFoundException($"Target guild with id '{id}' not found.");
        }

        private (Guild, User) GetGuildAndMamber(Guid id, string memberName)
        {
            var guild = GetWithKeys<Guild>(id) ?? throw new KeyNotFoundException($"Target guild with id '{id}' not found.");

            var member = Query<User>(u => u.Name.Equals(memberName)).SingleOrDefault() ?? throw new KeyNotFoundException($"Target user '{memberName}' not found.");

            return (guild, member);
        }

        public Guild Delete(Guid id)
        {
            return Remove(Get(id));
        }
    }
}