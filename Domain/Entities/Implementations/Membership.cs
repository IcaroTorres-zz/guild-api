using System;

namespace Domain.Entities
{
	public partial class Membership
	{
		public Membership(Guild guild, Member member)
		{
			Guild = guild;
			GuildId = guild.Id;
			Member = member;
			MemberId = member.Id;
		}

		public virtual Membership RegisterExit()
		{
			Until = DateTime.UtcNow;
			return this;
		}

		public virtual TimeSpan GetDuration()
		{
			return (Until ?? DateTime.UtcNow).Subtract(Since);
		}
	}
}