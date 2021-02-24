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

        public override Guild InviteMember(Member member)
        {
            return this;
        }

        internal override Guild RemoveMember(Member member)
        {
            return this;
        }

        internal override Guild AddMember(Member member)
        {
            return this;
        }

        public override Guild Promote(Member member)
        {
            return this;
        }

        public override Guild DemoteLeader()
        {
            return this;
        }

        internal override Membership ConfirmMembership(Member member)
        {
            return Membership.Null;
        }

        public override Guid Id { get => Guid.Empty; protected set { } }
        public override string Name { get => string.Empty; protected set { } }
    }
}