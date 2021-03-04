using Domain.Common;
using Domain.Nulls;
using Domain.States.Memberships;
using System;

namespace Domain.Models
{
    public class Membership : EntityModel<Membership>
    {
        public static readonly NullMembership Null = new NullMembership();

        internal Membership ChangeState(MembershipState state)
        {
            State = state;
            ModifiedDate = state.ModifiedDate;
            return this;
        }

        public virtual TimeSpan GetDuration()
        {
            return (ModifiedDate ?? DateTime.UtcNow).Subtract(CreatedDate);
        }

        public virtual Guid? MemberId { get; protected internal set; }
        public virtual Guid? GuildId { get; protected internal set; }

        private Guild _guild;
        public virtual Guild Guild { get => _guild ??= Guild.Null; protected internal set { _guild = value; } }

        private Member _member;
        public virtual Member Member { get => _member ??= Member.Null; protected internal set { _member = value; } }

        private MembershipState _state;
        internal virtual MembershipState State
        {
            get => _state ??= MembershipState.NewState(this, ModifiedDate);
            set => _state = value;
        }
    }
}