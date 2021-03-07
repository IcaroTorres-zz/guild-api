using Domain.Common;
using Domain.Models;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Domain.Nulls
{
    [ExcludeFromCodeCoverage]
    public sealed class NullMember : Member, INullObject
    {
        public override Member ChangeName(string newName)
        {
            return this;
        }

        internal override Membership ActivateMembership(Guild guild, IModelFactory factory)
        {
            return Membership.Null;
        }

        public override Guid Id { get => Guid.Empty; protected internal set { } }
        public override string Name { get => string.Empty; protected internal set { } }
        public override bool IsGuildLeader { get => false; protected set { } }
        public override Guid? GuildId { get => null; protected set { } }
        public override DateTime CreatedDate { get => default; protected internal set { } }
        public override DateTime? ModifiedDate { get => default; protected internal set { } }
        public override Guild GetGuild() => Guild.Null;
    }
}