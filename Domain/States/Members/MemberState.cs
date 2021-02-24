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
            guild.AddMember(Context);
            Context.ActivateMembership(guild);
            if (guild.Members.Count == 1)
            {
                Context.ReceiveLeadership(Member.Null);
                return Context.ChangeState(new GuildLeaderState(Context, guild));
            }
            return Context.ChangeState(new GuildMemberState(Context, guild));
        }
        internal virtual Member Leave()
        {
            Context.Guild.RemoveMember(Context);
            Context.ActiveMembership.BeFinished();
            return Context.ChangeState(new NoGuildMemberState(Context));
        }
        internal virtual Member BePromoted()
        {
            return Context.ChangeState(new GuildLeaderState(Context, Context.Guild));
        }
        internal virtual Member BeDemoted()
        {
            return Context.ChangeState(new GuildMemberState(Context, Context.Guild));
        }

        internal static MemberState NewState(Member member, Guild guild, bool isGuildLeader)
        {
            if (guild is INullObject) return new NoGuildMemberState(member);
            if (isGuildLeader) return new GuildLeaderState(member, guild);
            return new GuildMemberState(member, guild);
        }
    }
}
