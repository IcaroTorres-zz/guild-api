using Domain.Common;
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

        internal override Membership Join(Guild guild, IModelFactory factory)
        {
            Leave();
            return base.Join(guild, factory);
        }

        internal override Membership Leave()
        {
            Context.Guild.GetVice().State.BePromoted();
            BeDemoted();
            return base.Leave();
        }

        internal override Member BePromoted()
        {
            return Context;
        }

        internal override Member BeDemoted()
        {
            return Context.ChangeState(new GuildMemberState(Context, Context.Guild));
        }
    }
}
