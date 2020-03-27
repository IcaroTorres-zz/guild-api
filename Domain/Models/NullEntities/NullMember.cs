using DataAccess.Entities;

namespace Domain.Models.NullEntities
{
    public class NullMember : MemberModel
    {
        public NullMember() : base(new Member()) { }
        public override void ChangeName(string newName) { }
        public override MemberModel JoinGuild(InviteModel invite) => this;
        public override MemberModel BePromoted() => this;
        public override MemberModel BeDemoted() => this;
        public override MemberModel LeaveGuild() => this;
    }
}
