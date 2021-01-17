using System;

namespace Domain.Models.States.Memberships
{
    internal class ClosedMembershipState : MembershipState
	{
		public ClosedMembershipState(Membership context, DateTime modifiedDate) : base(context)
		{
			Context = context;
			ModifiedDate = modifiedDate;
		}

		internal override Membership Finish()
		{
			return Context;
		}
	}
}
