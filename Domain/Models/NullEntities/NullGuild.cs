using DataAccess.Entities;
using Domain.Validations;

namespace Domain.Models.NullEntities
{
    public class NullGuild : GuildModel
    {
        public NullGuild() : base(new Guild()) { }
        public override IValidationResult ValidationResult { get => new NotFoundValidationResult(nameof(GuildModel)); set {} }
        public override IValidationResult Validate() => ValidationResult;
        public override void ChangeName(string newName) { }
        public override InviteModel Invite(MemberModel member) => null;
        public override MemberModel AcceptMember(MemberModel member) => member;
        public override MemberModel Promote(MemberModel member) => member;
        public override MemberModel KickMember(MemberModel member) => member;
    }
}
