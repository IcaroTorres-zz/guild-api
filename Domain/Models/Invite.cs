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
            return State.BeAccepted(factory);
        }

        public virtual Invite BeDenied()
        {
            return State.BeDenied();
        }

        public virtual Invite BeCanceled()
        {
            return State.BeCanceled();
        }

        internal virtual Invite ChangeState(InviteState state)
        {
            State = state;
            Status = State.Status;
            return this;
        }

        public virtual InviteStatuses Status { get; protected internal set; }
        public virtual Guid? MemberId { get; protected internal set; }
        public virtual Guid? GuildId { get; protected internal set; }

        private Guild _guild;
        public virtual Guild Guild { get => _guild ??= Guild.Null; protected internal set { _guild = value; } }

        private Member _member;
        public virtual Member Member { get => _member ??= Member.Null; protected internal set { _member = value; } }

        private InviteState _state;
        internal virtual InviteState State
        {
            get => _state ??= InviteState.NewState(this, Guild, Member, Status);
            set => _state = value;
        }

        public HashSet<Invite> GetInvitesToCancel()
        {
            var cancellables = Guild.Invites
                .Where(x => x.Status == InviteStatuses.Pending &&
                            x.MemberId == Member.Id &&
                            x.GuildId == Guild.Id &&
                            x.Id != Id).ToHashSet();
            return cancellables;
        }
    }
}