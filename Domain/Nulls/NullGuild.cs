using Domain.Common;
using Domain.Models;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Domain.Nulls
{
    [ExcludeFromCodeCoverage]
    public sealed class NullGuild : Guild, INullObject
    {
        public override Guild ChangeName(string newName)
        {
            return this;
        }

        public override Invite InviteMember(Member member, IModelFactory factory)
        {
            return Invite.Null;
        }

        public override Membership RemoveMember(Member member)
        {
            return Membership.Null;
        }

        internal override Member AddMember(Member member)
        {
            return Member.Null;
        }

        public override Member Promote(Member member)
        {
            return Member.Null;
        }

        public override Member DemoteLeader()
        {
            return Member.Null;
        }

        public override Guid Id { get => Guid.Empty; protected internal set { } }
        public override string Name { get => string.Empty; protected internal set { } }
        public override DateTime CreatedDate { get => default; protected internal set { } }
        public override DateTime? ModifiedDate { get => default; protected internal set { } }
    }
}