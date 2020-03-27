using DataAccess.Entities;
using Domain.Validations;
using FluentValidation;
using System;
using System.Linq;
using System.Net;

namespace Domain.Models
{
    public class InviteModel : DomainModel<Invite>
    {
        public InviteModel(Invite entity) : base(entity) { }
        public InviteModel(GuildModel guild, MemberModel member) : base(new Invite())
        {
            Entity.Guild = guild.Entity;
            Entity.GuildId = guild.Entity.Id;
            Entity.Member = member.Entity;
            Entity.MemberId = member.Entity.Id;
        }
        public virtual InviteModel BeAccepted()
        {
            if (Entity.Status == InviteStatuses.Pending)
            {
                Entity.Status = InviteStatuses.Accepted;
                var memberModel = new MemberModel(Entity.Member);
                var guildModel = new GuildModel(Entity.Guild);
                guildModel.AcceptMember(memberModel.JoinGuild(this));
            }
            return this;
        }
        public virtual InviteModel BeDeclined()
        {
            if (Entity.Status == InviteStatuses.Pending)
            {
                Entity.Status = InviteStatuses.Declined;
            }
            return this;
        }
        public virtual InviteModel BeCanceled()
        {
            if (Entity.Status == InviteStatuses.Pending)
            {
                Entity.Status = InviteStatuses.Canceled;
            }
            return this;
        }
        public override IApiValidationResult Validate()
        {
            RuleFor(x => x.MemberId)
                .NotEmpty()
                .NotEqual(Guid.Empty)
                .WithErrorCode(((int)HttpStatusCode.Conflict).ToString());

            RuleFor(x => x.GuildId)
                .NotEmpty()
                .NotEqual(Guid.Empty)
                .WithErrorCode(((int)HttpStatusCode.Conflict).ToString());

            RuleFor(x => x.Member.Id)
                .Equal(x => x.MemberId)
                .Unless(x => x.Member == null)
                .WithErrorCode(((int)HttpStatusCode.Conflict).ToString());

            RuleFor(x => x.Guild.Id)
                .Equal(x => x.GuildId)
                .Unless(x => x.Guild == null)
                .WithErrorCode(((int)HttpStatusCode.Conflict).ToString());

            RuleFor(x => x.Status)
                .IsInEnum<Invite, InviteStatuses>()
                .WithErrorCode(((int)HttpStatusCode.BadRequest).ToString());

            RuleFor(x => x.Member.Memberships)
                .Must(x => x.Any(ms => ms.MemberId == Entity.MemberId && ms.GuildId == Entity.GuildId))
                .When(x => x.Status == InviteStatuses.Accepted && x.Member != null && x.Member.Memberships != null)
                .WithErrorCode(((int)HttpStatusCode.Conflict).ToString());

            return base.Validate();
        }
    }
}
