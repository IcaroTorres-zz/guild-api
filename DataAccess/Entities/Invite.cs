using DataAccess.Validations;
using Domain.Entities;
using Domain.Enums;
using Domain.Validations;
using Newtonsoft.Json;
using System;

namespace DataAccess.Entities
{
    [Serializable]
    public class Invite : BaseEntity, IInvite
    {
        // EF core suitable parametersless constructor hidden to be called elsewhere
        protected Invite() { }
        public Invite(Guild guild, Member member)
        {
            Guild = guild;
            GuildId = guild.Id;
            Member = member;
            MemberId = member.Id;
        }
        private InviteStatuses status = InviteStatuses.Pending;
        public virtual InviteStatuses Status
        {
            get => status;
            protected set
            {
                if (value != InviteStatuses.Pending)
                {
                    if (Status == InviteStatuses.Pending && value == InviteStatuses.Accepted)
                    {
                        BeAccepted();
                    }
                    status = value;
                }
            }
        }
        public Guid GuildId { get; protected set; }
        public Guid MemberId { get; protected set; }
        [JsonIgnore] public virtual Member Member { get; protected set; }
        [JsonIgnore] public virtual Guild Guild { get; protected set; }
        public virtual IInvite BeAccepted()
        {
            if (Status == InviteStatuses.Pending)
            {
                status = InviteStatuses.Accepted;
                Member.JoinGuild(this);
                Guild.AcceptMember(Member);
            }
            return this;
        }
        public virtual IInvite BeDeclined()
        {
            if (Status == InviteStatuses.Pending)
            {
                status = InviteStatuses.Declined;
            }
            return this;
        }
        public virtual IInvite BeCanceled()
        {
            if (Status == InviteStatuses.Pending)
            {
                status = InviteStatuses.Canceled;
            }
            return this;
        }
        public override IValidationResult Validate()
        {
            IErrorValidationResult result = null;
            if (Member == null)
            {
                result = new BadRequestValidationResult(nameof(Invite)).AddValidationErrors(nameof(Member), "Can't be null.");
            }

            if (!Member.IsValid)
            {
                result ??= new ConflictValidationResult(nameof(Invite));
                result.AddValidationErrors(nameof(Member), "Is Invalid.");
                foreach(var error in (Member.ValidationResult as IErrorValidationResult)?.Errors)
                {
                    result.AddValidationErrors(error.Key, error.Value.ToArray());
                }
            }

            if (Guild == null)
            {
                result ??= new ConflictValidationResult(nameof(Invite));
                result.AddValidationErrors(nameof(Guild), "Can't be null.");
            }

            if (!Guild.IsValid)
            {
                result ??= new ConflictValidationResult(nameof(Invite));
                result.AddValidationErrors(nameof(Guild), "Is Invalid.");
                foreach (var error in (Guild.ValidationResult as IErrorValidationResult)?.Errors)
                {
                    result.AddValidationErrors(error.Key, error.Value.ToArray());
                }
            }

            return result ?? ValidationResult;
        }
    }
}
