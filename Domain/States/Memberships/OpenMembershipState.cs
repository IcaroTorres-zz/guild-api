using Domain.Models;
using System;

namespace Domain.States.Memberships
{
    internal class OpenMembershipState : MembershipState
    {
        internal OpenMembershipState(Membership context) : base(context)
        {
            Context = context;
        }

        internal override Membership Finish()
        {
            return Context.ChangeState(new ClosedMembershipState(Context, DateTime.UtcNow));
        }
    }
}
