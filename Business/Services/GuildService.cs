using DataAccess.Context;
using Domain.DTOs;
using Domain.Entities;
using Domain.Models;
using Domain.Models.NullEntities;
using Domain.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Business.Services
{
    public class GuildService : BaseService, IGuildService
    {
        public GuildService(ApiContext context) : base(context) { }
        public GuildModel Create(GuildDto payload)
        {
            var masterEntity = GetByKeys<Member>((object)payload.MasterId);
            var masterModel = ModelFactory.ConstructWith<MemberModel, Member>(masterEntity);
            var newGuildModel = new GuildModel(payload.Name, masterModel);
            newGuildModel.Entity = Insert(newGuildModel);
            return newGuildModel;
        }
        public GuildModel Update(GuildDto payload, Guid id)
        {
            var guildModelToUpdate = Get(id);
            var masterEntity = GetByKeys<Member>((object)payload.MasterId);
            var masterModel = ModelFactory.ConstructWith<MemberModel, Member>(masterEntity);

            var currentMemberIds = guildModelToUpdate.Entity.Members.Select(x => x.Id);
            var receivedAlreadyMemberIds = payload.MemberIds.Intersect(currentMemberIds);
            var toInviteIds = payload.MemberIds.Except(receivedAlreadyMemberIds);
            var toKickIds = currentMemberIds.Except(receivedAlreadyMemberIds);

            Query<Member>(x => !x.Disabled)
                .Include(x => x.Memberships)
                .Include(x => x.Guild.Members)
                .Include(x => x.Guild.Invites)
                .Join(toInviteIds, x => x.Id, id => id, (member, _) => new MemberModel(member)).ToList()
                .ForEach(memberToInvite => guildModelToUpdate.Invite(memberToInvite)?.BeAccepted());

            guildModelToUpdate.Entity.Members
                .Join(toKickIds, x => x.Id, id => id, (member, _) => new MemberModel(member)).ToList()
                .ForEach(memberToKick => memberToKick.LeaveGuild());

            guildModelToUpdate.Promote(masterModel);
            guildModelToUpdate.ChangeName(payload.Name);
            guildModelToUpdate.Entity = Update(guildModelToUpdate);

            return guildModelToUpdate;
        }
        public IReadOnlyList<GuildModel> List(int count = 20)
        {
            return GetAll<Guild>(readOnly: true)
                .Take(count)
                .Select(ge => new GuildModel(ge))
                .ToList();
        }

        public GuildModel Get(Guid id, bool readOnly = false)
        {
            return ModelFactory.ConstructWith<GuildModel, Guild>(GetByKeys<Guild>((object)id));
        }

        public GuildModel Delete(Guid id)
        {
            var guildModel = Get(id);
            guildModel.Entity = Remove(guildModel);
            return guildModel;
        }
    }
}