using Domain.Entities;
using System;

namespace Domain.Models.NullEntities
{
    public class NullMember : MemberModel
    {
        public NullMember() : base(new Member { Id = Guid.Empty }) { }
        public override bool IsValid => false;
        public override void ChangeName(string newName) { }
        public override MemberModel JoinGuild(InviteModel invite) => this;
        public override MemberModel BePromoted() => this;
        public override MemberModel BeDemoted() => this;
        public override MemberModel LeaveGuild() => this;
    }
}
