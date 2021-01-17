using Domain.Models.States.Members;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Domain.Models.Nulls
{
    [ExcludeFromCodeCoverage]
    public sealed class NullMember : Member, INullObject
    {
		public NullMember()
		{
			ChangeState(new NoGuildMemberState(this));
		}

        public override Member ChangeName(string newName)
        {
            return this;
        }

        public override Member LeaveGuild()
        {
            return this;
        }

        internal override Member ReceiveLeadership(Member currentMaster)
        {
            return this;
        }

        internal override Member TransferLeadership(Member currentViceMaster)
        {
            return this;
        }

        internal override Member JoinGuild(Guild guild)
        {
            return this;
        }

        internal override Invite ConfirmGuildInvite(Guild guild)
        {
            return Invite.Null;
        }

		internal override Member ActivateMembership(Guild guild)
		{
			return this;
		}

        public override Guid Id { get => Guid.Empty; protected set { } }
        public override string Name { get => string.Empty; protected set { } }
        public override bool IsGuildLeader { get => false; protected set { } }
        public override Guid? GuildId { get => null; protected set { } }
        public override Guild Guild { get => Guild.Null; protected set { } }
    }
}