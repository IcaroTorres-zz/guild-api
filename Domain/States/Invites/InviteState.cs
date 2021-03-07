using Domain.Common;
using Domain.Enums;
using Domain.Models;
using System;

namespace Domain.States.Invites
{
    public abstract class InviteState
    {
        protected InviteState(Invite context)
        {
            Context = context;
        }

        internal Invite Context { get; set; }
        internal InviteStatuses Status { get; set; }
        internal DateTime? ModifiedDate { get; set; }
        internal abstract Membership BeAccepted(IModelFactory factory);
        internal abstract Invite BeDenied();
        internal abstract Invite BeCanceled();

        internal static InviteState NewState(Invite invite)
        {
            if (invite.GetGuild() is INullObject || invite.GetMember() is INullObject)
                return new ClosedInviteState(invite, invite.Status, invite.ModifiedDate);

            return invite.Status == InviteStatuses.Pending
                ? new OpenInviteState(invite)
                : new ClosedInviteState(invite, invite.Status, invite.ModifiedDate) as InviteState;
        }
    }
}
