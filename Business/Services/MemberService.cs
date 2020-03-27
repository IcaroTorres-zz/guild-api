using DataAccess.Context;
using Domain.DTOs;
using Domain.Entities;
using Domain.Models;
using Domain.Models.NullEntities;
using Domain.Services;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Business.Services
{
    public class MemberService : BaseService, IMemberService
    {
        public MemberService(ApiContext context) : base(context) { }

        public MemberModel Create(MemberDto payload)
        {
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
                var guildModel = ModelFactory.ConstructWith<GuildModel, Guild>(guildEntity);
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
            var memberModel = Get(id);
            payload.ApplyTo(memberModel.Entity);
            return memberModel;
        }
        public MemberModel Promote(Guid id)
        {
            return Get(id).BePromoted();
        }

        public MemberModel Demote(Guid id)
        {
            return Get(id).BeDemoted();
        }

        public MemberModel LeaveGuild(Guid id)
        {
            return Get(id).LeaveGuild();
        }

        public IReadOnlyList<MemberModel> List(MemberFilterDto payload)
        {
            return Query<Member>(x => x.Name.Contains(payload.Name) && (payload.GuildId == Guid.Empty || x.GuildId == payload.GuildId), readOnly: true)
                .Take(payload.Count)
                .Select(x => new MemberModel(x))
                .ToList();
        }
        public MemberModel Get(Guid memberId, bool readOnly = false)
        {
            return ModelFactory.ConstructWith<MemberModel, Member>(GetByKeys<Member>((object)memberId));
        }

        public MemberModel Delete(Guid id)
        {
            var memberModel = Get(id);
            memberModel.Entity = Remove(memberModel);
            return memberModel;
        }
    }
}