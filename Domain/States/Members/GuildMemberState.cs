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

        internal override Member Leave()
        {
            Context.GetActiveMembership().State.Finish();
            return Context.ChangeState(new NoGuildMemberState(Context));
        }

        internal override Member BePromoted()
        {
            return Context.ChangeState(new GuildLeaderState(Context, Context.Guild));
        }

        internal override Member BeDemoted()
        {
            return Context;
        }
    }
}
