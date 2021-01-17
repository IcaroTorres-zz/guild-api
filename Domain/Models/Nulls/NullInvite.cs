using Domain.Enums;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Domain.Models.Nulls
{
    [ExcludeFromCodeCoverage]
    public sealed class NullInvite : Invite, INullObject
    {
        public NullInvite()
        {
            Status = InviteStatuses.Canceled;
        }

        public override Invite BeAccepted()
        {
            return this;
        }

        public override Invite BeDenied()
        {
            return this;
        }

        public override Invite BeCanceled()
        {
            return this;
        }

        public override Guid Id { get => Guid.Empty; protected set { } }
        public override InviteStatuses Status { get => State.Status; protected set { } }
        public override Guid? MemberId { get => null; protected set { } }
        public override Guid? GuildId { get => null; protected set { } }
        public override Guild Guild { get => Guild.Null; protected set { } }
        public override Member Member { get => Member.Null; protected set { } }
		public override DateTime CreatedDate { get => DateTime.MaxValue; protected set { } }
    }
}