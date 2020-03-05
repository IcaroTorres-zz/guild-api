using Domain.Entities;
using Domain.Services;
using DataAccess.Context;
using Domain.DTOs;
using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Validations;
using System.Net;

namespace DataAccess.Services
{
    public class GuildService : BaseService, IGuildService
    {
        public GuildService(ApiContext context) : base(context) { }
        public IValidationResult Create(GuildDto payload)
        {
            if (!(Query<Guild>(p => p.Name.Equals(payload.Name)).SingleOrDefault() is Guild guild))
            {
                if (GetMember(payload.MasterId) is Member master)
                {
                    guild = new Guild(payload.Name, master);
                    if (guild.IsValid)
                    {
                        guild = Insert(guild);
                    }
                    return guild.ValidationResult;
                }
                return new NotFoundValidationResult($"A {nameof(Member)} with given {nameof(Member.Id)} '{payload.MasterId}' do not exists.");
            }
            return new ConflictValidationResult($"A {nameof(Guild)} with given name '{payload.Name}' already exists.");          
        }
        public IValidationResult Update(GuildDto payload, Guid id)
        {
            if (GetGuild(id) is Guild guildToUpdate)
            {
                if (GetMember(payload.MasterId) is Member master)
                {
                    guildToUpdate.Promote(master);
                    guildToUpdate.ChangeName(payload.Name);
                    guildToUpdate.UpdateMembers(
                        Query<Member>(m => !m.Disabled, included: $"{nameof(Member.Memberships)},{nameof(Member.Guild)}")
                        .Join(payload.MemberIds, m => m.Id, id => id, (member, _) => member));

                    return guildToUpdate.ValidationResult;
                }
                return new NotFoundValidationResult($"A {nameof(Member)} with given {nameof(Member.Id)} '{payload.MasterId}' do not exists.");
            }
            return new NotFoundValidationResult($"A {nameof(Guild)} with given {nameof(Guild.Id)} '{id}' do not exists.");
        }
        public IReadOnlyList<IGuild> List(int count = 20)
        {
            return GetAll<Guild>(included: nameof(Guild.Members), readOnly: true).Take(count).ToList();
        }
        public IGuild GetGuild(Guid id)
        {
            var keys = new object[] { id };
            var collections = new[] { nameof(Guild.Members), nameof(Guild.Invites) };
            return GetWithKeys<Guild>(keys, collections: collections);
        }
        private IMember GetMember(Guid memberId)
        {
            var keys = new object[] { memberId };
            var navigations = new[] { nameof(Member.Guild) };
            var collections = new[] { nameof(Member.Memberships) };
            return GetWithKeys<Member>(keys, navigations, collections);
        }
        public IValidationResult Delete(Guid id)
        {
            var guildToRemove = GetGuild(id);
            if (guildToRemove.IsValid)
            {
                guildToRemove = Remove(guildToRemove);
            }
            return guildToRemove.ValidationResult;
        }
    }
}