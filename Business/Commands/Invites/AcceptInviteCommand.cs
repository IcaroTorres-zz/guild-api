using System;
using Domain.Repositories;

namespace Business.Commands.Invites
{
	public class AcceptInviteCommand : PatchInviteCommand
	{
		public AcceptInviteCommand(Guid id, IInviteRepository repository) : base(id, repository)
		{
		}
	}
}