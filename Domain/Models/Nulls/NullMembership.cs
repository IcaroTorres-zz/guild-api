using Domain.Models.States.Memberships;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Domain.Models.Nulls
{
    [ExcludeFromCodeCoverage]
    public sealed class NullMembership : Membership, INullObject
    {
		public NullMembership()
		{
			ChangeState(new ClosedMembershipState(this, DateTime.UtcNow));
		}
        public override TimeSpan GetDuration()
        {
            return TimeSpan.Zero;
        }

        internal override Membership BeFinished()
        {
            return this;
        }

        public override Guid Id { get => Guid.Empty; protected set { } }
        public override Guid? MemberId { get => null; protected set { } }
        public override Guid? GuildId { get => null; protected set { } }
        public override Guild Guild { get => Guild.Null; protected set { } }
        public override Member Member { get => Member.Null; protected set { } }
        public override DateTime CreatedDate { get => default; protected set { } }
        public override DateTime? ModifiedDate { get => null; protected set { } }
    }
}