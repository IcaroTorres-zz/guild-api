using Domain.Entities;
using Domain.Services;
using DataAccess.Context;
using Domain.DTOs;
using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using DataAccess.Validations;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Services
{
    public class GuildService : BaseService, IGuildService
    {
        public GuildService(ApiContext context) : base(context) {}
        public IGuild Create(GuildDto payload)
        {
            if (Query<Guild>(p => p.Name.Equals(payload.Name), included: nameof(Guild.Members)).SingleOrDefault() is Guild guild)
            {
                guild.ValidationResult = new ConflictValidationResult(nameof(Guild))
                    .AddValidationErrors(nameof(Guild), $"With given name '{payload.Name}' already exists.");
                    return guild;
            }
            var master = GetMember(payload.MasterId) as Member;
            var newGuild = Insert(new Guild(payload.Name, master));
            return newGuild;
        }
        public IGuild Update(GuildDto payload, Guid id)
        {
            var guildToUpdate = Get(id) as Guild;
            var master = GetMember(payload.MasterId);

            if (!guildToUpdate.IsGuildMember(master))
            {
                var guildConflict = new ConflictValidationResult(nameof(Guild))
                    .AddValidationErrors(nameof(Guild.Members), $"Cannot {nameof(Guild.Promote)} a non {nameof(Member)}.");

                var memberConflict = new ConflictValidationResult(nameof(Member))
                    .AddValidationErrors(nameof(Member.Guild), $"Cannot {nameof(Member.BePromoted)} due to not being member of target {nameof(Member.Guild)}.");

                guildToUpdate.ValidationResult = guildConflict;
                master.ValidationResult = memberConflict;
            }
            
            guildToUpdate.Promote(master);
            guildToUpdate.ChangeName(payload.Name);
            
            var currentMemberIds = guildToUpdate.Members.Select(m => m.Id);
            var receivedAlreadyMemberIds = payload.MemberIds.Intersect(currentMemberIds);
            var toInviteIds = payload.MemberIds.Except(receivedAlreadyMemberIds);
            var toKickIds = currentMemberIds.Except(receivedAlreadyMemberIds);

            var memberQuery = Query<Member>(m => !m.Disabled)
                .Include(g => g.Memberships)
                .Include(m => m.Guild.Members)
                .Include(m => m.Guild.Invites);

            memberQuery.Join(toInviteIds, m => m.Id, id => id, (member, _) => member).ToList()
                       .ForEach(memberToInvite => Insert<Invite>(guildToUpdate.Invite(memberToInvite)?.BeAccepted() as Invite));
            memberQuery.Join(toInviteIds, m => m.Id, id => id, (member, _) => member).ToList()
                       .ForEach(memberToKick => memberToKick.LeaveGuild());

            return guildToUpdate;
        }
        public IReadOnlyList<IGuild> List(int count = 20)
        {
            return GetAll<Guild>(included: nameof(Guild.Members), readOnly: true).Take(count).ToList();
        }
        public IGuild Get(Guid id) => GetGuild(id);
        public IGuild Delete(Guid id) => Remove(Get(id));
    }
}