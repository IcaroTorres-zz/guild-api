using Domain.Validations;
using System;
using DataAccess.Entities;

namespace Domain.Models.NullEntities
{
    public class NullInvite : InviteModel
    {
        public NullInvite() : base(new Invite()) { }
        public override InviteModel BeAccepted() => this;
        public override InviteModel BeDeclined() => this;
        public override InviteModel BeCanceled() => this;
        public override IValidationResult ValidationResult { get => new BadRequestValidationResult(nameof(Invite)); set {} }
        public override IValidationResult Validate() => ValidationResult;
    }
}
