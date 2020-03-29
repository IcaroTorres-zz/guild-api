using Domain.Entities;
using System;

namespace Domain.Models.NullEntities
{
    public class NullMembership : MembershipModel
    {
        public NullMembership() : base(new Membership { Id = Guid.Empty }) { }
        public override bool IsValid => false;
        public override MembershipModel RegisterExit() => this;
        public override TimeSpan GetDuration() => DateTime.UtcNow.Subtract(DateTime.UtcNow);
    }
}
