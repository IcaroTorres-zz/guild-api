using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Domain.Entities
{
	[Serializable]
	public partial class Member : EntityModel<Member>
	{
		public Member()
		{
		}

		public virtual string Name { get; internal set; }
		public virtual bool IsGuildMaster { get; internal set; }
		public virtual Guid? GuildId { get; internal set; }
		[JsonIgnore] public virtual Guild Guild { get; internal set; }
		[JsonIgnore] public virtual ICollection<Membership> Memberships { get; internal set; } = new List<Membership>();
	}
}