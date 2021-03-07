using Domain.Models;
using System;

namespace Domain.States.Memberships
{
    internal class ClosedMembershipState : MembershipState
    {
        public ClosedMembershipState(Membership context, DateTime? modifiedDate) : base(context)
        {
            ModifiedDate = modifiedDate;
        }

        internal override Membership Finish()
        {
            return Context;
        }
    }
}
