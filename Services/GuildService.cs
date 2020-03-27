using DataAccess.Context;
using DataAccess.Entities;
using Domain.DTOs;
using Domain.Models;
using Domain.Models.NullEntities;
using Domain.Validations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Services
{
    public class GuildService : BaseService, IGuildService
    {
        public GuildService(ApiContext context) : base(context) { }
        public GuildModel Create(GuildDto payload)
        {
            //if (Query<Guild>(p => p.Name.Equals(payload.Name), true).SingleOrDefault() is Guild guild)
            //{
            //    return new GuildModel(guild)
            //    {
            //        ValidationResult = new ConflictValidationResult(nameof(Guild))
            //        .AddValidationErrors(nameof(Guild), $"With given name '{payload.Name}' already exists.")
            //    };
            //}
            var masterEntity = GetByKeys<Member>((object)payload.MasterId);
            var masterModel = masterEntity is Member ? new MemberModel(masterEntity) : new NullMember();
            var newGuildModel = new GuildModel(payload.Name, masterModel);
            newGuildModel.Entity = Insert(newGuildModel);

            return newGuildModel;
        }
        public GuildModel Update(GuildDto payload, Guid id)
        {
            var guildModelToUpdate = Get(id);
            var masterEntity = GetByKeys<Member>((object)payload.MasterId);
            var masterModel = masterEntity is Member ?new MemberModel(masterEntity) : new NullMember();

            //if (!guildModelToUpdate.IsGuildMember(masterModel))
            //{
            //    var guildConflict = new ConflictValidationResult(nameof(Guild))
            //        .AddValidationErrors(nameof(Guild.Members), $"Cannot {nameof(GuildModel.Promote)} a non {nameof(Member)}.");

            //    var memberConflict = new ConflictValidationResult(nameof(Member))
            //        .AddValidationErrors(nameof(Member.Guild), $"Cannot {nameof(MemberModel.BePromoted)} due to not being member of target {nameof(Member.Guild)}.");

            //    guildModelToUpdate.ValidationResult = guildConflict;
            //    masterModel.ValidationResult = memberConflict;
            //}

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
        public IReadOnlyList<GuildModel> List(int count = 20) => GetAll<Guild>(readOnly: true)
            .Take(count)
            .Select(ge => new GuildModel(ge))
            .ToList();

        public GuildModel Get(Guid id, bool readOnly = false)
        {
            var guildEntity = GetByKeys<Guild>((object)id);
            return guildEntity is Guild ? new GuildModel(guildEntity) : new NullGuild();
        }

        public GuildModel Delete(Guid id)
        {
            var guildModel = Get(id);
            guildModel.Entity = Remove(guildModel);
            return guildModel;
        }
    }
}