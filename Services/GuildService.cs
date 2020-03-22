using DataAccess.Context;
using Domain.DTOs;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Validations;
using Microsoft.EntityFrameworkCore;
using DataAccess.Repositories;
using DataAccess.Entities;

namespace Services
{
    public class GuildService : BaseRepository, IGuildService
    {
        public GuildService(ApiContext context) : base(context) {}
        public GuildModel Create(GuildDto payload)
        {
            if (Query<Guild>(p => p.Name.Equals(payload.Name), true).SingleOrDefault() is Guild guild)
            {
                var conflictGuildModel = new GuildModel(guild);
                conflictGuildModel.ValidationResult = new ConflictValidationResult(nameof(Guild))
                    .AddValidationErrors(nameof(Guild), $"With given name '{payload.Name}' already exists.");
                    return conflictGuildModel;
            }
            var masterModel = new MemberModel(GetMember(payload.MasterId));
            var newGuildModel = new GuildModel(payload.Name, masterModel);
            newGuildModel.Entity = Insert<Guild>(newGuildModel);
            
            return newGuildModel;
        }
        public GuildModel Update(GuildDto payload, Guid id)
        {
            var guildModelToUpdate = Get(id);
            var masterModel = new MemberModel(GetMember(payload.MasterId));

            if (!guildModelToUpdate.IsGuildMember(masterModel))
            {
                var guildConflict = new ConflictValidationResult(nameof(Guild))
                    .AddValidationErrors(nameof(Guild.Members), $"Cannot {nameof(GuildModel.Promote)} a non {nameof(Member)}.");

                var memberConflict = new ConflictValidationResult(nameof(Member))
                    .AddValidationErrors(nameof(Member.Guild), $"Cannot {nameof(MemberModel.BePromoted)} due to not being member of target {nameof(Member.Guild)}.");

                guildModelToUpdate.ValidationResult = guildConflict;
                masterModel.ValidationResult = memberConflict;
            }
            
            guildModelToUpdate.Promote(masterModel);
            guildModelToUpdate.ChangeName(payload.Name);
            
            var currentMemberIds = guildModelToUpdate.Entity.Members.Select(m => m.Id);
            var receivedAlreadyMemberIds = payload.MemberIds.Intersect(currentMemberIds);
            var toInviteIds = payload.MemberIds.Except(receivedAlreadyMemberIds);
            var toKickIds = currentMemberIds.Except(receivedAlreadyMemberIds);

            Query<Member>(m => !m.Disabled)
                .Include(g => g.Memberships)
                .Include(m => m.Guild.Members)
                .Include(m => m.Guild.Invites)
                .Join(toInviteIds, m => m.Id, id => id, (member, _) => new MemberModel(member)).ToList()
                .ForEach(memberToInvite => guildModelToUpdate.Invite(memberToInvite)?.BeAccepted());

            guildModelToUpdate.Entity.Members
                .Join(toKickIds, m => m.Id, id => id, (member, _) => new MemberModel(member)).ToList()
                .ForEach(memberToKick => memberToKick.LeaveGuild());

            guildModelToUpdate.Entity = Update<Guild>(guildModelToUpdate);

            return guildModelToUpdate;
        }
        public IReadOnlyList<GuildModel> List(int count = 20) => GetAll<Guild>(included: nameof(Guild.Members), readOnly: true)
            .Take(count)
            .Select(ge => new GuildModel(ge))
            .ToList();
        public GuildModel Get(Guid id, bool readOnly = false) => new GuildModel(GetGuild(id, readOnly));
        public GuildModel Delete(Guid id) 
        {
            var guildModel = Get(id);
            guildModel.Entity = Remove<Guild>(guildModel);
            return guildModel;
        }
    }
}