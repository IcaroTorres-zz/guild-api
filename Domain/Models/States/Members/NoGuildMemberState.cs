namespace Domain.Models.States.Members
{
    internal class NoGuildMemberState : MemberState
	{
		internal NoGuildMemberState(Member context) : base(context)
		{
		}

		internal override Guild Guild => Guild.Null;

		internal override Member Leave()
		{
			return Context;
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
