using Domain.Models;

namespace Domain.States.Members
{
    internal class GuildLeaderState : MemberState
    {
        internal GuildLeaderState(Member context, Guild guild) : base(context)
        {
            Guild = guild;
        }

        internal override bool IsGuildLeader => true;

        internal override Member Join(Guild guild)
        {
            base.Leave();
            return base.Join(guild);
        }

        internal override Member BePromoted()
        {
            return Context;
        }
    }
}
