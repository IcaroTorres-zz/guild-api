using Domain.DTOs;
using Domain.Entities;
using Domain.Models;
using Domain.Models.NullEntities;
using Domain.Repositories;
using Domain.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Business.Services
{
    public class GuildService : IGuildService
    {
        private readonly IGuildRepository GuildRepository;
        private readonly IMemberRepository MemberRepository;

        public GuildService(IGuildRepository guilds,
            IMemberRepository members,
            IInviteRepository invites)
        {
            GuildRepository = guilds;
            MemberRepository = members;
        }
        public GuildModel Create(GuildDto payload)
        {
            var masterEntity = MemberRepository.Get(payload.MasterId);
            var masterModel = ModelFactory.ConstructWith<MemberModel, Member>(masterEntity);
            var newGuildModel = new GuildModel(payload.Name, masterModel);
            newGuildModel.Entity = GuildRepository.Insert(newGuildModel);
            return newGuildModel;
        }
        public GuildModel Update(GuildDto payload, Guid id)
        {
            var guildModelToUpdate = Get(id);
            var masterEntity = MemberRepository.Get(payload.MasterId);
            var masterModel = ModelFactory.ConstructWith<MemberModel, Member>(masterEntity);

            var currentMemberIds = guildModelToUpdate.Entity.Members.Select(x => x.Id);
            var receivedAlreadyMemberIds = payload.MemberIds.Intersect(currentMemberIds);
            var toInviteIds = payload.MemberIds.Except(receivedAlreadyMemberIds);
            var toKickIds = currentMemberIds.Except(receivedAlreadyMemberIds);

            MemberRepository.Query(x => !x.Disabled)
                .Include(x => x.Memberships)
                .Include(x => x.Guild.Members)
                .Include(x => x.Guild.Invites)
                .Join(toInviteIds, x => x.Id, id => id, (member, _) => new MemberModel(member)).ToList()
                .ForEach(memberToInvite => guildModelToUpdate.Invite(memberToInvite)?.BeAccepted());

            guildModelToUpdate.Entity.Members
                .Join(toKickIds, x => x.Id, id => id, (member, _) => new MemberModel(member)).ToList()
                .ForEach(memberToKick => memberToKick.LeaveGuild());

            guildModelToUpdate.ChangeName(payload.Name);
            guildModelToUpdate.Promote(masterModel);
            guildModelToUpdate.Entity = GuildRepository.Update(guildModelToUpdate);

            return guildModelToUpdate;
        }
        public IReadOnlyList<GuildModel> List(int count = 20)
        {
            return GuildRepository.GetAll(readOnly: true)
                .Take(count)
                .Select(ge => new GuildModel(ge))
                .ToList();
        }

        public GuildModel Get(Guid id, bool readOnly = false)
        {
            return ModelFactory.ConstructWith<GuildModel, Guild>(GuildRepository.Get(id));
        }

        public GuildModel Delete(Guid id)
        {
            var guildModel = Get(id);
            guildModel.Entity = GuildRepository.Remove(guildModel);
            return guildModel;
        }
    }
}