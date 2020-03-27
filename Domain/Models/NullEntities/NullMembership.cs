using Domain.Entities;
using Domain.Validations;
using System;

namespace Domain.Models.NullEntities
{
    public class NullMembership : MembershipModel
    {
        public NullMembership() : base(new Membership()) { }
        public override bool IsValid => false;
        public override MembershipModel RegisterExit()
        {
            return this;
        }

        public override TimeSpan GetDuration()
        {
            return DateTime.UtcNow.Subtract(DateTime.UtcNow);
        }
        public override IApiValidationResult Validate()
        {
            var validator = new Invalidator<Membership>(Entity);
            RuleFor(x => x).SetValidator(validator);
            return validator.Validate();
        }
    }
}
