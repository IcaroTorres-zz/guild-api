using System;
using Business.Responses;
using Domain.Entities;
using MediatR;

namespace Business.Commands.Invites
{
	public class InviteMemberCommand : IRequest<ApiResponse<Invite>>, ITransactionalCommand
	{
		public Guid MemberId { get; set; }
		public Guid GuildId { get; set; }
	}
}