using Domain.Entities;
using System;

namespace Domain.Models.NullEntities
{
    public class NullInvite : InviteModel
    {
        public NullInvite() : base(new Invite { Id = Guid.Empty }) { }
        public override bool IsValid => false;
        public override InviteModel BeAccepted() => this;
        public override InviteModel BeDeclined() => this;
        public override InviteModel BeCanceled() => this;
    }
}
