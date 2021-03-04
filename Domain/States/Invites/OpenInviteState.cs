using Domain.Common;
using Domain.Enums;
using Domain.Models;

namespace Domain.States.Invites
{
    internal class OpenInviteState : InviteState
    {
        internal OpenInviteState(Invite context) : base(context)
        {
            Context = context;
            Status = InviteStatuses.Pending;
        }

        internal override Membership BeAccepted(IModelFactory factory)
        {
            var membership = Context.Member.State.Join(Context.Guild, factory);
            Context.ChangeState(new ClosedInviteState(Context, InviteStatuses.Accepted));
            return membership;
        }

        internal override Invite BeDenied()
        {
            return Context.ChangeState(new ClosedInviteState(Context, InviteStatuses.Denied));
        }

        internal override Invite BeCanceled()
        {
            return Context.ChangeState(new ClosedInviteState(Context, InviteStatuses.Canceled));
        }
    }
}
