using Domain.DTOs;
using Domain.Entities;
using Domain.Models;
using Domain.Repositories;
using Domain.Services;
using FluentValidation;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Linq;

namespace Business.Services
{
    public class MemberService : IMemberService
    {
        private readonly IMemberRepository _memberRepository;
        private readonly IGuildRepository _guildRepository;
        private readonly IValidator<Member> _memberValidator;
        private readonly ModelFactory _modelFactory;

        public MemberService(IMemberRepository members, 
            IGuildRepository guilds,
            IValidator<Member> validator,
            ModelFactory factory)
        {
            _memberRepository = members;
            _guildRepository = guilds;
            _memberValidator = validator;
            _modelFactory = factory;
        }

        public MemberModel Get(Guid memberId, bool readOnly = false)
        {
            return (MemberModel) _modelFactory
                .Create(_memberRepository.Get(memberId))
                .ApplyValidator(_memberValidator);
        }

        public Pagination<Member> List(MemberFilterDto payload)
        {
            var query = _memberRepository.Query(x => x.Name.Contains(payload.Name) 
                && (payload.GuildId == Guid.Empty || x.GuildId == payload.GuildId), readOnly: true);

            var totalCount = query.Count();
            var validEntities = query.Take(payload.Count)
                .Select(x => _modelFactory.Create(x)).ToList()
                .Where(i => i.ApplyValidator(_memberValidator).IsValid)
                .Select(x => x.Entity).ToList();

            return new Pagination<Member>(validEntities, totalCount, payload.Count);
        }

        public MemberModel Create(MemberDto payload)
        {
            var memberModel = new MemberModel(payload.Name);
            memberModel.Entity = _memberRepository.Insert(memberModel.ApplyValidator(_memberValidator));

            return memberModel;
        }

        public MemberModel Update(MemberDto payload, Guid id)
        {
            var memberModel = Get(id);
            memberModel.ChangeName(payload.Name);
            if (payload.GuildId is Guid guildId && guildId != Guid.Empty)
            {
                var guildEntity = _guildRepository.Get(guildId);
                var guildModel = _modelFactory.Create(guildEntity);
                guildModel.Invite(memberModel).BeAccepted();
            }
            else
            {
                memberModel.LeaveGuild();
            }
            memberModel.Entity = _memberRepository.Update(memberModel.ApplyValidator(_memberValidator));

            return memberModel;
        }
        public MemberModel Patch(Guid id, JsonPatchDocument<Member> payload)
        {
            var memberModel = Get(id);
            payload.ApplyTo(memberModel.Entity);
            memberModel.ApplyValidator(_memberValidator);
            return memberModel;
        }
        public MemberModel Promote(Guid id) => (MemberModel) Get(id).BePromoted().ApplyValidator(_memberValidator);

        public MemberModel Demote(Guid id) => (MemberModel) Get(id).BeDemoted().ApplyValidator(_memberValidator);

        public MemberModel LeaveGuild(Guid id) => (MemberModel) Get(id).LeaveGuild().ApplyValidator(_memberValidator);

        public MemberModel Delete(Guid id)
        {
            var memberModel = Get(id);
            memberModel.Entity = _memberRepository.Remove(memberModel);
            return memberModel;
        }
    }
}