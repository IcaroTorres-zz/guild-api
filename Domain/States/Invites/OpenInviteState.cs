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

        internal override Invite BeAccepted()
        {
            Context.Member.JoinGuild(Context.Guild);
            return Context.ChangeState(new ClosedInviteState(Context, InviteStatuses.Accepted));
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
