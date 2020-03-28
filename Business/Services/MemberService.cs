using Domain.DTOs;
using Domain.Entities;
using Domain.Models;
using Domain.Models.NullEntities;
using Domain.Repositories;
using Domain.Services;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Business.Services
{
    public class MemberService : IMemberService
    {
        private readonly IMemberRepository MemberRepository;
        private readonly IGuildRepository GuildRepository;

        public MemberService(IMemberRepository members, IGuildRepository guilds)
        {
            MemberRepository = members;
            GuildRepository = guilds;
        }

        public MemberModel Create(MemberDto payload)
        {
            var memberModel = new MemberModel(payload.Name);
            memberModel.Entity = MemberRepository.Insert(memberModel);

            return memberModel;
        }
        public MemberModel Update(MemberDto payload, Guid id)
        {
            var memberModel = Get(id);
            memberModel.ChangeName(payload.Name);
            if (payload.GuildId is Guid guildId && guildId != Guid.Empty)
            {
                var guildEntity = GuildRepository.Get(guildId);
                var guildModel = ModelFactory.ConstructWith<GuildModel, Guild>(guildEntity);
                guildModel.Invite(memberModel).BeAccepted();
            }
            else
            {
                memberModel.LeaveGuild();
            }
            memberModel.Entity = MemberRepository.Update(memberModel);

            return memberModel;
        }
        public MemberModel Patch(Guid id, JsonPatchDocument<Member> payload)
        {
            var memberModel = Get(id);
            payload.ApplyTo(memberModel.Entity);
            return memberModel;
        }
        public MemberModel Promote(Guid id) => Get(id).BePromoted();

        public MemberModel Demote(Guid id) => Get(id).BeDemoted();

        public MemberModel LeaveGuild(Guid id) => Get(id).LeaveGuild();

        public IReadOnlyList<MemberModel> List(MemberFilterDto payload)
        {
            return MemberRepository.Query(x => x.Name.Contains(payload.Name)
                && (payload.GuildId == Guid.Empty || x.GuildId == payload.GuildId),
                readOnly: true)
                .Take(payload.Count)
                .Select(x => new MemberModel(x))
                .ToList();
        }
        public MemberModel Get(Guid memberId, bool readOnly = false)
        {
            return ModelFactory.ConstructWith<MemberModel, Member>(MemberRepository.Get(memberId));
        }
        public MemberModel Delete(Guid id)
        {
            var memberModel = Get(id);
            memberModel.Entity = MemberRepository.Remove(memberModel);
            return memberModel;
        }
    }
}