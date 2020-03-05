using Domain.Entities;
using Domain.Services;
using DataAccess.Context;
using Domain.DTOs;
using DataAccess.Entities;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAccess.Services
{
    public class MemberService : BaseService, IMemberService
    {
        public MemberService(ApiContext context) : base(context) { }

        public IMember Create(MemberDto payload)
        {
            if (!(Query<Member>(p => p.Name.Equals(payload.Name)).SingleOrDefault() is Member guild))
            {
                return Insert(new Member(payload.Name));
            }
            throw new Exception($"A {nameof(Member)} with given name '{payload.Name}' already exists.");
        }

        public IMember Update(MemberDto payload, Guid id)
        {
            var member = GetMember(id);

            member.ChangeName(payload.Name);

            if (payload.GuildId is Guid guildId && guildId != Guid.Empty)
            {
                var guild = GetGuild(guildId);
                member.JoinGuild(guild.Invite(member));
                guild.AcceptMember(member);
            }
            else
            {
                member.LeaveGuild();
            }
            return member;
        }

        public IMember Patch(Guid id, JsonPatchDocument<Member> payload)
        {
            var member = GetMember(id) as Member;

            payload.ApplyTo(member);

            return member;
        }

        public IMember Promote(Guid id)
        {
            return GetMember(id).BePromoted();
        }

        public IMember Demote(Guid id)
        {
            return GetMember(id).BeDemoted();
        }

        public IMember LeaveGuild(Guid id)
        {
            return GetMember(id).LeaveGuild();
        }
        public IReadOnlyList<IMember> List(MemberFilterDto payload) => Query<Member>(m
            => m.Name.Contains(payload.Name)
            && (payload.GuildId == Guid.Empty || m.GuildId == payload.GuildId),
            included: $"{nameof(Member.Memberships)},{nameof(Member.Guild)}",
            readOnly: true).Take(payload.Count).ToList();

        public IMember GetMember(Guid memberId)
        {
            var keys = new object[] { memberId };
            var navigations = new[] { nameof(Member.Guild) };
            var collections = new[] { nameof(Member.Memberships) };

            return GetWithKeys<Member>(keys, navigations, collections)
                ?? throw new KeyNotFoundException($"Target {nameof(Member)} with id '{memberId}' not found.");
        }

        private IGuild GetGuild(Guid guildId)
        {
            var keys = new object[] { guildId };
            var collections = new[] { nameof(Guild.Members), nameof(Guild.Invites) };

            return GetWithKeys<Guild>(keys, collections: collections);
        }

        public IMember Delete(Guid id)
        {
            return Remove<Member>(id);
        }
    }
}