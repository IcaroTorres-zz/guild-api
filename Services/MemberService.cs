using DataAccess.Context;
using Domain.DTOs;
using DataAccess.Entities;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Validations;
using DataAccess.Repositories;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Services
{
    public class MemberService : BaseRepository, IMemberService
    {
        public MemberService(ApiContext context) : base(context) { }

        public MemberModel Create(MemberDto payload)
        {
            if ((Query<Member>(p => p.Name.Equals(payload.Name)).SingleOrDefault() is Member member))
            {
                var conflictMemberModel = new MemberModel(member);
                conflictMemberModel.ValidationResult = new ConflictValidationResult(nameof(Member))
                    .AddValidationErrors(nameof(Member.Name), $"With given name '{payload.Name}' already exists.");
                return conflictMemberModel;
            }
            var memberModel = new MemberModel(payload.Name);
            memberModel.Entity = Insert(memberModel);

            return memberModel;
        }
        public MemberModel Update(MemberDto payload, Guid id)
        {
            var memberModel = Get(id);
            memberModel.ChangeName(payload.Name);
            if (payload.GuildId is Guid guildId && guildId != Guid.Empty)
            {
                var guildModel = new GuildModel(GetGuild(guildId));
                guildModel.Invite(memberModel).BeAccepted();
            }
            else
            {
                memberModel.LeaveGuild();
            }
            memberModel.Entity = Update<Member>(memberModel);
            
            return memberModel;
        }
        public MemberModel Patch(Guid id, JsonPatchDocument<Member> payload)
        {
            var memberEntity = GetMember(id);

            payload.ApplyTo(memberEntity);

            return new MemberModel(memberEntity);
        }
        public MemberModel Promote(Guid id) => Get(id).BePromoted();
        public MemberModel Demote(Guid id) => Get(id).BeDemoted();
        public MemberModel LeaveGuild(Guid id) => Get(id).LeaveGuild();
        public IReadOnlyList<MemberModel> List(MemberFilterDto payload)
        => Query<Member>(m => m.Name.Contains(payload.Name) && (payload.GuildId == Guid.Empty || m.GuildId == payload.GuildId), readOnly: true)
            .Take(payload.Count)
            .Select(m => new MemberModel(m))
            .ToList();
        public MemberModel Get(Guid memberId, bool readOnly = false) => new MemberModel(GetMember(memberId, readOnly));
        public MemberModel Delete(Guid id) 
        {
            var memberModel = Get(id);
            memberModel.Entity = Remove<Member>(memberModel);
            return memberModel;
        }
    }
}