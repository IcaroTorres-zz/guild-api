using Domain.Common;
using Domain.Enums;
using Domain.Nulls;
using Domain.States.Invites;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Models
{
    public class Invite : EntityModel<Invite>
    {
        public static readonly NullInvite Null = new NullInvite();

        public virtual Membership BeAccepted(IModelFactory factory)
        {
            return GetState().BeAccepted(factory);
        }

        public virtual Invite BeDenied()
        {
            return GetState().BeDenied();
        }

        public virtual Invite BeCanceled()
        {
            return GetState().BeCanceled();
        }

        internal virtual Invite ChangeState(InviteState newState)
        {
            state = newState;
            Status = newState.Status;
            ModifiedDate = newState.ModifiedDate;
            return this;
        }

        public virtual InviteStatuses Status { get; protected internal set; }
        public virtual Guid? MemberId { get; protected internal set; }
        public virtual Guid? GuildId { get; protected internal set; }

        internal Guild guild;
        public virtual Guild GetGuild() => guild ?? Guild.Null;

        internal Member member;
        public virtual Member GetMember() => member ?? Member.Null;

        protected InviteState state;
        internal virtual InviteState GetState()
        {
            state ??= InviteState.NewState(this);
            return state;
        }

        public IReadOnlyCollection<Invite> GetInvitesToCancel()
        {
            var cancellables = GetGuild().Invites
                .Where(x => x.Status == InviteStatuses.Pending &&
                            x.MemberId == MemberId &&
                            x.GuildId == GuildId &&
                            x.Id != Id).ToArray();
            return cancellables;
        }
    }
}