using Domain.Enums;

namespace Domain.Models.States.Invites
{
	internal class ClosedInviteState : InviteState
	{
		internal ClosedInviteState(Invite context, InviteStatuses status) : base(context)
		{
			Context = context;
			Status = status;
		}

		internal override Invite BeAccepted()
		{
			return Context;
		}

		internal override Invite BeDenied()
		{
			return Context;
		}
		internal override Invite BeCanceled()
		{
			return Context;
		}
	}
}
