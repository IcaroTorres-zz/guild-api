using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Domain.Entities
{
	[Serializable]
	public partial class Guild : EntityModel<Guild>
	{
		public Guild()
		{
		}

		public virtual string Name { get; internal set; }
		[JsonIgnore] public virtual ICollection<Member> Members { get; internal set; } = new List<Member>();
		[JsonIgnore] public virtual ICollection<Invite> Invites { get; internal set; } = new List<Invite>();
	}
}