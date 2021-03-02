using Domain.Common;
using Domain.Models;

namespace Domain.States.Members
{
    public abstract class MemberState
    {
        protected MemberState(Member context)
        {
            Context = context;
        }

        internal Member Context { get; set; }
        internal virtual Guild Guild { get; set; }
        internal virtual bool IsGuildLeader => false;

        internal virtual Member Join(Guild guild)
        {
            Context.ActivateMembership(guild);
            guild.AddMember(Context);
            var nextState = guild.Members.Count == 1 ? new GuildLeaderState(Context, guild) as MemberState
                                                     : new GuildMemberState(Context, guild);
            return Context.ChangeState(nextState);
        }

        internal abstract Member Leave();

        internal abstract Member BePromoted();

        internal abstract Member BeDemoted();

        internal static MemberState NewState(Member member)
        {
            if (member.Guild is INullObject) return new NoGuildMemberState(member);
            return member.IsGuildLeader
                ? new GuildLeaderState(member, member.Guild)
                : new GuildMemberState(member, member.Guild) as MemberState;
        }
    }
}
