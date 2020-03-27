using DataAccess.Entities;

namespace Domain.Models.NullEntities
{
    public class NullGuild : GuildModel
    {
        public NullGuild() : base(new Guild()) { }
        public override void ChangeName(string newName) { }
        public override InviteModel Invite(MemberModel member) => new NullInvite();
        public override MemberModel AcceptMember(MemberModel member) => member;
        public override MemberModel Promote(MemberModel member) => member;
        public override MemberModel KickMember(MemberModel member) => member;
    }
}
