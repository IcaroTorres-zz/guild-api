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
            var guild = GetWithKeys<Guild>(id) ?? throw new KeyNotFoundException($"Target guild with id '{id}' not found.");

            payload.ApplyTo(guild);

            return guild;
        }

        public Guild AddMember(Guid id, string memberName)
        {
            var guild = Get(id);

            var member = GetMember(memberName);

            return guild.InviteToGuild(member);
        }

        public Guild RemoveMember(Guid id, string memberName)
        {
            var guild = Get(id);

            var member = GetMember(memberName);

            return guild.KickMember(member);
        }

        public Guild ChangeGuildMaster(Guid id, string memberName)
        {
            var guild = Get(id);

            var member = GetMember(memberName);

            return guild.PromoteToGuildMaster(member);
        }

        public IQueryable<Guild> List(int count = 20)
        {
            return GetAll<Guild>().Take(count);
        }

        public Guild Get(Guid id)
        {
            return GetWithKeys<Guild>(id) ?? throw new KeyNotFoundException($"Target guild with id '{id}' not found.");
        }

        private User GetMember(string memberName)
        {
            return Query<User>(u => u.Name.Equals(memberName)).SingleOrDefault() ?? throw new KeyNotFoundException($"Target user '{memberName}' not found.");
        }

        public Guild Delete(Guid id)
        {
            var guild = Get(id);

            return Remove(guild);
        }
    }
}