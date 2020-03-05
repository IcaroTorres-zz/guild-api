using Domain.Entities;
using Domain.Enums;
using Domain.Validations;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net;

namespace DataAccess.Entities
{
    [Serializable]
    public class Invite : BaseEntity, IInvite
    {
        // EF core suitable parametersless constructor hidden to be called elsewhere
        protected Invite()
        {
            ValidationResult = new OkValidationResult(this);
        }
        public Invite(Guild guild, Member member) : base()
        {
            Guild = guild;
            Member = member;
        }
        private InviteStatuses status = InviteStatuses.Pending;
        public InviteStatuses Status
        {
            get => status;
            protected set
            {
                if (value != InviteStatuses.Pending)
                {
                    if (Status == InviteStatuses.Pending && value == InviteStatuses.Accepted)
                    {
                        Member.JoinGuild(this);
                        Guild.AcceptMember(Member);
                    }
                    status = value;
                }
            }
        }
        public Guid GuildId { get; protected set; }
        public Guid MemberId { get; protected set; }
        [JsonIgnore] public virtual Member Member { get; protected set; }
        [JsonIgnore] public virtual Guild Guild { get; protected set; }
        public IInvite BeAccepted()
        {
            if (Status == InviteStatuses.Pending)
            {
                Member.JoinGuild(this);
                Guild.AcceptMember(Member);
                status = InviteStatuses.Accepted;
            }
            return this;
        }
        public IInvite BeDeclined()
        {
            if (Status == InviteStatuses.Pending)
            {
                status = InviteStatuses.Declined;
            }
            return this;
        }
        public IInvite BeCanceled()
        {
            if (Status == InviteStatuses.Pending)
            {
                status = InviteStatuses.Canceled;
            }
            return this;
        }
        public override IValidationResult Validate()
        {
            IValidationResult result = null;
            if (Member == null)
            {
                result = new BadRequestValidationResult($"{Member} Can't be null.");
            }

            if (Member.IsValid)
            {
                if (result == null)
                {
                    result = new ConflictValidationResult($"{nameof(Member)} is Invalid.");
                }
                foreach(var error in Member.ValidationResult.Errors)
                {
                    result.AddValidationError(error.Status, error.Message);
                }
            }

            if (Guild == null)
            {
                var errorMessage = $"{nameof(Guild)} Can't be null.";

                if (result == null)
                    result = new ConflictValidationResult(errorMessage);
                else
                    result.AddValidationError(HttpStatusCode.Conflict, errorMessage);
            }

            if (Guild.IsValid)
            {
                if (result == null)
                {
                    result = new ConflictValidationResult($"{nameof(Guild)} is Invalid.");
                }
                foreach (var error in Guild.ValidationResult.Errors)
                {
                    result.AddValidationError(error.Status, error.Message);
                }
            }

            return result ?? ValidationResult;
        }
    }
}
