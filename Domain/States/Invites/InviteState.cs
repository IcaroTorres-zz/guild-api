using Domain.Common;
using Domain.Enums;
using Domain.Models;

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
        internal abstract Invite BeAccepted();
        internal abstract Invite BeDenied();
        internal abstract Invite BeCanceled();

        internal static InviteState NewState(Invite invite, Guild guild, Member member, InviteStatuses status)
        {
            if (guild is INullObject || member is INullObject) return new ClosedInviteState(invite, status);

            if (status == InviteStatuses.Pending) return new OpenInviteState(invite);

            return new ClosedInviteState(invite, status);
        }
    }
}
