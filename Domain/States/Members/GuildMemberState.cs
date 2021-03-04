using Domain.Common;
using Domain.Models;

namespace Domain.States.Members
{
    internal class GuildMemberState : MemberState
    {
        internal GuildMemberState(Member context, Guild guild) : base(context)
        {
            Guild = guild;
        }

        internal override Membership Join(Guild guild, IModelFactory factory)
        {
            Leave();
            return base.Join(guild, factory);
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
