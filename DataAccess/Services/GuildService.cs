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
                    .AddValidationError(nameof(Guild), $"With given name '{payload.Name}' already exists.");
                    return guild;
            }
            var master = GetMember(payload.MasterId) as Member;
            var newGuild = Insert(new Guild(payload.Name, master));
            return newGuild;
        }
        public IGuild Update(GuildDto payload, Guid id)
        {
            var guildToUpdate = Get(id);
            var master = GetMember(payload.MasterId);
            if (!guildToUpdate.IsGuildMember(master))
            {
                var guildConflict = new ConflictValidationResult(nameof(Guild))
                    .AddValidationError(nameof(Guild.Members), $"Cannot {nameof(Guild.Promote)} a non {nameof(Member)}.");

                var memberConflict = new ConflictValidationResult(nameof(Member))
                    .AddValidationError(nameof(Member.Guild), $"Cannot {nameof(Member.BePromoted)} due to not being member of target {nameof(Member.Guild)}.");

                guildToUpdate.ValidationResult = guildConflict;
                master.ValidationResult = memberConflict;
            }
            guildToUpdate.Promote(master);
            guildToUpdate.ChangeName(payload.Name);
            guildToUpdate.UpdateMembers(Query<Member>(m => !m.Disabled)
                .Include(m => m.Guild.Members)
                .Include(m => m.Guild.Invites)
                .Include(g => g.Memberships)
                .Join(payload.MemberIds, m => m.Id, id => id, (member, _) => member));

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