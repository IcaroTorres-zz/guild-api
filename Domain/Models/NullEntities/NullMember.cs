using DataAccess.Entities;
using Domain.Validations;

namespace Domain.Models.NullEntities
{
    public class NullMember : MemberModel
    {
        public NullMember() : base(new Member()) { }
        public override IValidationResult ValidationResult { get => new NotFoundValidationResult(nameof(Member)); set { } }
        public override IValidationResult Validate() => ValidationResult;
        public override void ChangeName(string newName) { }
        public override MemberModel JoinGuild(InviteModel invite) => this;
        public override MemberModel BePromoted() => this;
        public override MemberModel BeDemoted() => this;
        public override MemberModel LeaveGuild() => this;
    }
}
