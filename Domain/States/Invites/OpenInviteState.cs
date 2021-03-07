using Domain.Common;
using Domain.Enums;
using Domain.Models;
using System;

namespace Domain.States.Invites
{
    internal class OpenInviteState : InviteState
    {
        internal OpenInviteState(Invite context) : base(context)
        {
            Status = InviteStatuses.Pending;
        }

        internal override Membership BeAccepted(IModelFactory factory)
        {
            var guild = Context.GetGuild();
            var membership = Context.GetMember().GetState().Join(guild, factory);
            Context.ChangeState(new ClosedInviteState(Context, InviteStatuses.Accepted, DateTime.UtcNow));
            return membership;
        }

        internal override Invite BeDenied()
        {
            return Context.ChangeState(new ClosedInviteState(Context, InviteStatuses.Denied, DateTime.UtcNow));
        }

        internal override Invite BeCanceled()
        {
            return Context.ChangeState(new ClosedInviteState(Context, InviteStatuses.Canceled, DateTime.UtcNow));
        }
    }
}
