using Domain.Common;
using Domain.Models;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Domain.Nulls
{
    [ExcludeFromCodeCoverage]
    public sealed class NullMembership : Membership, INullObject
    {
        public override TimeSpan GetDuration()
        {
            return TimeSpan.Zero;
        }

        public override Guid Id { get => Guid.Empty; protected internal set { } }
        public override Guid? MemberId { get => null; protected internal set { } }
        public override Guid? GuildId { get => null; protected internal set { } }
        public override string GuildName { get => string.Empty; protected internal set { } }
        public override string MemberName { get => string.Empty; protected internal set { } }
        public override DateTime CreatedDate { get => default; protected internal set { } }
        public override DateTime? ModifiedDate { get => default; protected internal set { } }
    }
}