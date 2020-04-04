using System;
using Business.ResponseOutputs;
using Domain.Entities;
using Domain.Repositories;
using MediatR;

namespace Business.Commands.Invites
{
	public abstract class PatchInviteCommand : IRequest<ApiResponse<Invite>>
	{
		protected PatchInviteCommand(Guid id, IInviteRepository repository)
		{
			Id = id;
			Invite = repository.GetForAcceptOperation(id).Result;
		}

		public Guid Id { get; }
		public Invite Invite { get; }
	}
}