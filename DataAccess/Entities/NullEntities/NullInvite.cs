using DataAccess.Validations;
using Domain.Entities;
using Domain.Enums;
using Domain.Validations;
using System;

namespace DataAccess.Entities.NullEntities
{
    [Serializable]
    public class NullInvite : Invite
    {
        public NullInvite()
        {
            Id = Guid.Empty;
            GuildId = Guid.Empty;
            MemberId = Guid.Empty;
            Guild = new NullGuild();
            Member = new NullMember();
        }
        public override InviteStatuses Status
        {
            get => InviteStatuses.Pending;
            protected set { }
        }
        public override IInvite BeAccepted() => this;
        public override IInvite BeDeclined() => this;
        public override IInvite BeCanceled() => this;
        public override IValidationResult ValidationResult { get => new BadRequestValidationResult(nameof(Invite)); set {} }
        public override IValidationResult Validate() => ValidationResult;
    }
}
