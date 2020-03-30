using Domain.DTOs;
using Domain.Entities;
using Domain.Models;
using Domain.Repositories;
using Domain.Services;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Business.Services
{
    public class GuildService : IGuildService
    {
        private readonly IGuildRepository _guildRepository;
        private readonly IMemberRepository _memberRepository;
        private readonly ModelFactory _modelFactory;
        private readonly IValidator<Guild> _guildValidator;

        public GuildService(IGuildRepository guilds,
            IMemberRepository members,
            ModelFactory factory,
            IValidator<Guild> validator)
        {
            _guildRepository = guilds;
            _memberRepository = members;
            _modelFactory = factory;
            _guildValidator = validator;
        }

        public GuildModel Get(Guid id, bool readOnly = false)
        {
            return (GuildModel) _modelFactory.Create(_guildRepository.Get(id)).ApplyValidator(_guildValidator);
        }

        public Pagination<Guild> List(int count = 20)
        {
            var query = _guildRepository.GetAll(readOnly: true);
            var totalCount = query.Count();
            var validEntities = query.Take(count)
                .Select(x => _modelFactory.Create(x)).ToList()
                .Where(i => i.ApplyValidator(_guildValidator).IsValid)
                .Select(x => x.Entity).ToList();

            return new Pagination<Guild>(validEntities, totalCount, count);
        }

        public GuildModel Create(GuildDto payload)
        {
            var masterEntity = _memberRepository.Get(payload.MasterId);
            var masterModel = _modelFactory.Create(masterEntity);
            var guildModel = new GuildModel(payload.Name, masterModel);
            guildModel.Entity = _guildRepository.Insert(guildModel.ApplyValidator(_guildValidator));
            return guildModel;
        }

        public GuildModel Update(GuildDto payload, Guid id)
        {
            var guildModelToUpdate = Get(id);
            var masterEntity = _memberRepository.Get(payload.MasterId);
            var masterModel = _modelFactory.Create(masterEntity);

            var currentMemberIds = guildModelToUpdate.Entity.Members.Select(x => x.Id);
            var receivedAlreadyMemberIds = payload.MemberIds.Intersect(currentMemberIds);
            var idsToInvite = payload.MemberIds.Except(receivedAlreadyMemberIds);
            var idsToKick = currentMemberIds.Except(receivedAlreadyMemberIds);

            _memberRepository.Query(x => !x.Disabled)
                .Join(idsToInvite, x => x.Id, id => id, (member, _) => _modelFactory.Create(member)).ToList()
                .ForEach(memberModelToInvite => guildModelToUpdate.Invite(memberModelToInvite)?.BeAccepted());

            guildModelToUpdate.Entity.Members
                .Join(idsToKick, x => x.Id, id => id, (member, _) => _modelFactory.Create(member)).ToList()
                .ForEach(memberModelToKick => memberModelToKick.LeaveGuild());

            guildModelToUpdate.ChangeName(payload.Name);
            guildModelToUpdate.Promote(masterModel);
            guildModelToUpdate.Entity = _guildRepository.Update(
                guildModelToUpdate.ApplyValidator(_guildValidator));

            return guildModelToUpdate;
        }

        public GuildModel Delete(Guid id)
        {
            var guildModel = Get(id);
            guildModel.Entity = _guildRepository.Remove(guildModel);
            return guildModel;
        }
    }
}