using Domain.Entities;
using Domain.Validations;

namespace Domain.Models.NullEntities
{
    public class NullInvite : InviteModel
    {
        public NullInvite() : base(new Invite()) { }
        public override bool IsValid => false;
        public override InviteModel BeAccepted()
        {
            return this;
        }

        public override InviteModel BeDeclined()
        {
            return this;
        }

        public override InviteModel BeCanceled()
        {
            return this;
        }

        public override IApiValidationResult Validate()
        {
            var validator = new Invalidator<Invite>(Entity);
            RuleFor(x => x).SetValidator(validator);
            return validator.Validate();
        }
    }
}
