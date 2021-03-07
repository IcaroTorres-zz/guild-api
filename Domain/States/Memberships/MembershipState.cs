using Domain.Models;
using System;

namespace Domain.States.Memberships
{
    public abstract class MembershipState
    {
        protected MembershipState(Membership context)
        {
            Context = context;
        }

        internal Membership Context { get; set; }
        internal DateTime? ModifiedDate { get; set; }
        internal abstract Membership Finish();

        internal static MembershipState NewState(Membership membership)
        {
            return membership.ModifiedDate.HasValue
                ? new ClosedMembershipState(membership, membership.ModifiedDate) as MembershipState
                : new OpenMembershipState(membership);
        }
    }
}
