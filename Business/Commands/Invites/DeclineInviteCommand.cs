using System;
using Domain.Repositories;

namespace Business.Commands.Invites
{
	public class DeclineInviteCommand : PatchInviteCommand
	{
		public DeclineInviteCommand(Guid id, IInviteRepository repository) : base(id, repository)
		{
		}
	}
}