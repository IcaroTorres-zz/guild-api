using Domain.Models;

namespace Domain.States.Members
{
    internal class NoGuildMemberState : MemberState
    {
        internal NoGuildMemberState(Member context) : base(context)
        {
        }

        internal override Guild Guild => Guild.Null;

        internal override Membership Leave()
        {
            return Membership.Null;
        }

        internal override Member BePromoted()
        {
            return Context;
        }

        internal override Member BeDemoted()
        {
            return Context;
        }
    }
}
