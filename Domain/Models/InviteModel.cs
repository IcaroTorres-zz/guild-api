using Domain.Validations;
using Domain.Enums;
using System;
using DataAccess.Entities;

namespace Domain.Models
{
    public class InviteModel : DomainModel<Invite>
    {
        public InviteModel(Invite entity) : base(entity) { }
        public InviteModel(GuildModel guild, MemberModel member) : base(new Invite())
        {
            Entity.Guild = guild.Entity;
            Entity.Member = member.Entity;
        }
        public virtual InviteModel BeAccepted()
        {
            if (Entity.Status == InviteStatuses.Pending)
            {
                Entity.Status = InviteStatuses.Accepted;
                var guildModel = new GuildModel(Entity.Guild);
                var memberModel = new MemberModel(Entity.Member);
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
        public override IValidationResult Validate()
        {
            IErrorValidationResult result = null;
            if (Entity.Member == null)
            {
                result = new BadRequestValidationResult(nameof(Invite)).AddValidationErrors(nameof(Member), "Can't be null.");
            }

            var memberModel = new MemberModel(Entity.Member);
            if (!memberModel.IsValid)
            {
                result ??= new ConflictValidationResult(nameof(Invite));
                result.AddValidationErrors(nameof(Member), "Is Invalid.");
                foreach(var error in (memberModel.ValidationResult as IErrorValidationResult)?.Errors)
                {
                    result.AddValidationErrors(error.Key, error.Value.ToArray());
                }
            }

            if (Entity.Guild == null)
            {
                result ??= new ConflictValidationResult(nameof(Invite));
                result.AddValidationErrors(nameof(Guild), "Can't be null.");
            }

            var guildModel = new GuildModel(Entity.Guild);
            if (!guildModel.IsValid)
            {
                result ??= new ConflictValidationResult(nameof(Invite));
                result.AddValidationErrors(nameof(Guild), "Is Invalid.");
                foreach (var error in (guildModel.ValidationResult as IErrorValidationResult)?.Errors)
                {
                    result.AddValidationErrors(error.Key, error.Value.ToArray());
                }
            }

            return result ?? ValidationResult;
        }
    }
}
