using Domain.Entities;
using Domain.Validations;

namespace Domain.Models.NullEntities
{
    public class NullGuild : GuildModel
    {
        public NullGuild() : base(new Guild()) { }
        public override bool IsValid => false;
        public override void ChangeName(string newName) { }
        public override InviteModel Invite(MemberModel member)
        {
            return new NullInvite();
        }

        public override MemberModel AcceptMember(MemberModel member)
        {
            return member;
        }

        public override MemberModel Promote(MemberModel member)
        {
            return member;
        }

        public override MemberModel KickMember(MemberModel member)
        {
            return member;
        }
        public override IApiValidationResult Validate()
        {
            var validator = new Invalidator<Guild>(Entity);
            RuleFor(x => x).SetValidator(validator);
            return validator.Validate();
        }
    }
}
