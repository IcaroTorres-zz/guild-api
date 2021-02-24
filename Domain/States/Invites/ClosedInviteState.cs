using Domain.Enums;
using Domain.Models;

namespace Domain.States.Invites
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
