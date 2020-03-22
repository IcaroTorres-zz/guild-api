using Domain.Validations;
using JetBrains.Annotations;
using System;
using System.Linq;
using DataAccess.Entities;

namespace Domain.Models
{
    public class MemberModel : DomainModel<Member>
    {
        public MemberModel(Member entity) : base(entity) { }
        public MemberModel([NotNull] string name) : base(new Member { Name = name }) { }
        public virtual void ChangeName([NotNull] string newName)
        {
            Entity.Name = newName;
        }
        public virtual MemberModel JoinGuild([NotNull] InviteModel invite)
        {
            if (invite is InviteModel receivedInvite
                && receivedInvite.Entity.Guild is Guild invitingGuild
                && invitingGuild != Entity.Guild
                && receivedInvite.Entity.Status == Domain.Enums.InviteStatuses.Accepted)
            {
                LeaveGuild();
                Entity.Guild = invitingGuild;
                Entity.Memberships.Add(new Membership
                { 
                    GuildId = invitingGuild.Id,
                    MemberId = Entity.Id
                });
            }
            return this;
        }
        public virtual MemberModel BePromoted()
        {
            if (Entity.Guild is Guild)
            {
                Entity.Guild.Members
                     .Where(m => m.IsGuildMaster && m.Id != Entity.Id)
                     .ToList()
                     .ForEach(m => m.IsGuildMaster = false);

                Entity.IsGuildMaster = true;
            }
            return this;
        }
        public virtual MemberModel BeDemoted()
        {
            if (Entity.IsGuildMaster && Entity.Guild is Guild)
            {
                if (Entity.Guild.Members?.Count > 1)
                {
                    Entity.IsGuildMaster = false;

                    var newMaster = Entity.Guild.Members
                        .OrderByDescending(m => new MembershipModel(m.Memberships
                            .SingleOrDefault(ms => ms.Exit == null))
                            ?.GetDuration())
                        .FirstOrDefault(m => m.Id != Entity.Id && !m.IsGuildMaster);

                    if (newMaster is Member)
                        new MemberModel(newMaster).BePromoted();
                }
                else
                {
                    var memberTypeName = nameof(Member);
                    var guildTypeName = nameof(Guild);
                    var conflictKey = $"{guildTypeName}.{nameof(Guild.Members)}";
                    var conflictMessages = new string [2]
                    {
                        $"Can not {nameof(BeDemoted)} due to be the last {memberTypeName} left in {guildTypeName}.",
                        $"Consider leaving the {guildTypeName}."
                    };
                    ValidationResult = new ConflictValidationResult(memberTypeName).AddValidationErrors(conflictKey, conflictMessages);
                }
            }
            return this;
        }
        public virtual MemberModel LeaveGuild()
        {
            if (Entity.Guild is Guild)
            {
                var membership = Entity.Memberships
                    .OrderBy(ms => ms.Entrance)
                    .LastOrDefault();

                if (membership is Membership)    
                    new MembershipModel(membership).RegisterExit();

                new GuildModel(Entity.Guild).KickMember(this);
                Entity.Guild = null;
            }
            return this;
        }
        public override IValidationResult Validate()
        {
            return string.IsNullOrWhiteSpace(Entity.Name)
                ? new BadRequestValidationResult(nameof(Member)).AddValidationErrors(nameof(Entity.Name), "Can't be null or empty.")
                : ValidationResult;
        }
    }
}
