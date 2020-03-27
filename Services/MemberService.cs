using DataAccess.Context;
using DataAccess.Entities;
using Domain.DTOs;
using Domain.Models;
using Domain.Models.NullEntities;
using Domain.Validations;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Services
{
    public class MemberService : BaseService, IMemberService
    {
        public MemberService(ApiContext context) : base(context) { }

        public MemberModel Create(MemberDto payload)
        {
            //if ((Query<Member>(p => p.Name.Equals(payload.Name)).SingleOrDefault() is Member member))
            //{
            //    return new MemberModel(member)
            //    {
            //        ValidationResult = new ConflictValidationResult(nameof(Member))
            //        .AddValidationErrors(nameof(Member.Name), $"With given name '{payload.Name}' already exists.")
            //    };
            //}
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
                var guildEntity = GetByKeys<Guild>((object)guildId);
                var guildModel = guildEntity is Guild ? new GuildModel(guildEntity) : new NullGuild();
                guildModel.Invite(memberModel).BeAccepted();
            }
            else
            {
                memberModel.LeaveGuild();
            }
            memberModel.Entity = Update(memberModel);

            return memberModel;
        }
        public MemberModel Patch(Guid id, JsonPatchDocument<Member> payload)
        {
            if (GetByKeys<Member>((object)id) is Member memberEntity)
            {
                payload.ApplyTo(memberEntity);

                return new MemberModel(memberEntity);
            }

            return new NullMember();
        }
        public MemberModel Promote(Guid id) => Get(id).BePromoted();
        public MemberModel Demote(Guid id) => Get(id).BeDemoted();
        public MemberModel LeaveGuild(Guid id) => Get(id).LeaveGuild();
        public IReadOnlyList<MemberModel> List(MemberFilterDto payload)
        {
            return Query<Member>(x => x.Name.Contains(payload.Name) && (payload.GuildId == Guid.Empty || x.GuildId == payload.GuildId), readOnly: true)
                .Take(payload.Count)
                .Select(x => new MemberModel(x))
                .ToList();
        }
        public MemberModel Get(Guid memberId, bool readOnly = false)
        {
            return GetByKeys<Member>((object)memberId) is Member memberEntity
                ? new MemberModel(memberEntity) : new NullMember();
        }

        public MemberModel Delete(Guid id)
        {
            var memberModel = Get(id);
            memberModel.Entity = Remove(memberModel);
            return memberModel;
        }
    }
}