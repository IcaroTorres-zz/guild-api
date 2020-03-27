using Domain.Entities;
using Domain.Validations;
using FluentValidation;
using JetBrains.Annotations;
using System;
using System.Linq;

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
                && receivedInvite.Entity.Status == InviteStatuses.Accepted)
            {
                LeaveGuild();
                Entity.Guild = invitingGuild;
                Entity.GuildId = invitingGuild.Id;
                Entity.Memberships.Add(new Membership
                {
                    Guild = Entity.Guild,
                    GuildId = Entity.GuildId.Value,
                    Member = Entity,
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
                     .Where(x => x.IsGuildMaster && x.Id != Entity.Id)
                     .ToList()
                     .ForEach(x => x.IsGuildMaster = false);

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
                        .OrderByDescending(x => new MembershipModel(x.Memberships.SingleOrDefault(x => x.Until == null))?.GetDuration())
                        .FirstOrDefault(x => x.Id != Entity.Id && !x.IsGuildMaster);

                    if (newMaster is Member)
                        new MemberModel(newMaster).BePromoted();
                }
            }
            return this;
        }
        public virtual MemberModel LeaveGuild()
        {
            if (Entity.Guild is Guild)
            {
                var membership = Entity.Memberships
                    .OrderBy(x => x.Since)
                    .LastOrDefault();

                if (membership is Membership)
                    new MembershipModel(membership).RegisterExit();

                new GuildModel(Entity.Guild).KickMember(this);
                Entity.Guild = null;
            }
            return this;
        }
        public override IApiValidationResult Validate()
        {
            RuleFor(x => x.Name).NotEmpty();

            RuleFor(x => x.GuildId)
                .NotEmpty()
                .NotEqual(Guid.Empty)
                .Unless(x => x.Guild == null);

            RuleFor(x => x.Guild.Id)
                .Equal(x => x.GuildId.Value)
                .Unless(x => x.Guild == null);

            RuleFor(x => x.Memberships)
                .Must(x => x.Any(ms
                    => ms.MemberId == Entity.Id
                    && ms.GuildId == Entity.GuildId
                    && ms.Until == null
                    && !ms.Disabled))
                .When(x => x.Memberships.Any() && Entity.GuildId != null);

            return base.Validate();
        }
    }
}
