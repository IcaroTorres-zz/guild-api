using Domain.Common;
using Domain.Enums;
using Domain.Models;
using System;

namespace Domain.States.Invites
{
    internal class ClosedInviteState : InviteState
    {
        internal ClosedInviteState(Invite context, InviteStatuses status, DateTime? modifiedDate) : base(context)
        {
            Status = status;
            ModifiedDate = modifiedDate;
        }

        internal override Membership BeAccepted(IModelFactory factory)
        {
            return Membership.Null;
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
