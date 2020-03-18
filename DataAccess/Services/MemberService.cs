using Domain.Entities;
using Domain.Services;
using DataAccess.Context;
using Domain.DTOs;
using DataAccess.Entities;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using DataAccess.Validations;

namespace DataAccess.Services
{
    public class MemberService : BaseService, IMemberService
    {
        public MemberService(ApiContext context) : base(context) { }

        public IMember Create(MemberDto payload)
        {
            if ((Query<Member>(p => p.Name.Equals(payload.Name)).SingleOrDefault() is Member guild))
            {
                guild.ValidationResult = new ConflictValidationResult(nameof(Member))
                    .AddValidationErrors(nameof(Member), $"With given name '{payload.Name}' already exists.");
                return guild;
            }
            return Insert(new Member(payload.Name));
        }
        public IMember Update(MemberDto payload, Guid id)
        {
            var member = Get(id);
            member.ChangeName(payload.Name);
            if (payload.GuildId is Guid guildId && guildId != Guid.Empty)
            {
                GetGuild(guildId).Invite(member).BeAccepted();
            }
            else
            {
                member.LeaveGuild();
            }
            return member;
        }
        public IMember Patch(Guid id, JsonPatchDocument<Member> payload)
        {
            var member = Get(id) as Member;

            payload.ApplyTo(member);

            return member;
        }
        public IMember Promote(Guid id) => Get(id).BePromoted();
        public IMember Demote(Guid id) => Get(id).BeDemoted();
        public IMember LeaveGuild(Guid id) => Get(id).LeaveGuild();
        public IReadOnlyList<IMember> List(MemberFilterDto payload)
            => Query<Member>(m => m.Name.Contains(payload.Name)
                               && (payload.GuildId == Guid.Empty 
                               || m.GuildId == payload.GuildId), 
                            readOnly: true).Take(payload.Count).ToList();
        public IMember Get(Guid memberId, bool readOnly = false) => GetMember(memberId, readOnly);
        public IMember Delete(Guid id) => Remove<Member>(id);
    }
}