using System;

namespace Domain.Models.States.Memberships
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

		internal static MembershipState NewState(Membership membership, DateTime? modifiedDate)
		{
			return modifiedDate == null
				? new OpenMembershipState(membership)
				: new ClosedMembershipState(membership, modifiedDate.Value) as MembershipState;
		}
	}
}
