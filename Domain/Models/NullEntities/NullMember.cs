using Domain.Entities;
using Domain.Validations;

namespace Domain.Models.NullEntities
{
    public class NullMember : MemberModel
    {
        public NullMember() : base(new Member()) { }
        public override bool IsValid => false;
        public override void ChangeName(string newName) { }
        public override MemberModel JoinGuild(InviteModel invite)
        {
            return this;
        }

        public override MemberModel BePromoted()
        {
            return this;
        }

        public override MemberModel BeDemoted()
        {
            return this;
        }

        public override MemberModel LeaveGuild()
        {
            return this;
        }
        public override IApiValidationResult Validate()
        {
            var validator = new Invalidator<Member>(Entity);
            RuleFor(x => x).SetValidator(validator);
            return validator.Validate();
        }
    }
}
