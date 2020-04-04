using System;
using Domain.Repositories;

namespace Business.Commands.Invites
{
	public class CancelInviteCommand : PatchInviteCommand
	{
		public CancelInviteCommand(Guid id, IInviteRepository repository) : base(id, repository)
		{
		}
	}
}