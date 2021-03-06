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

        internal virtual Membership Join(Guild guild, IModelFactory factory)
        {
            var membership = Context.ActivateMembership(guild, factory);
            guild.AddMember(Context);
            var nextState = guild.Members.Count == 1 ? new GuildLeaderState(Context, guild) as MemberState
                                                     : new GuildMemberState(Context, guild);
            Context.ChangeState(nextState);
            return membership;
        }

        internal virtual Membership Leave()
        {
            var finishedMembership = Context.GetActiveMembership().State.Finish();
            Context.ChangeState(new NoGuildMemberState(Context));
            return finishedMembership;
        }

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
