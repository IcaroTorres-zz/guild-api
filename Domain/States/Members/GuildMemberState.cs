using Domain.Models;

namespace Domain.States.Members
{
    internal class GuildMemberState : MemberState
    {
        internal GuildMemberState(Member context, Guild guild) : base(context)
        {
            Guild = guild;
        }

        internal override Member Join(Guild guild)
        {
            Leave();
            return base.Join(guild);
        }

        internal override Member BeDemoted()
        {
            return Context;
        }
    }
}
