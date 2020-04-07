using System.Threading;
using System.Threading.Tasks;
using Business.Commands.Invites;
using Business.Responses;
using Domain.Entities;
using MediatR;

namespace Business.Handlers.Invites
{
	public class CancelInviteHandler : IRequestHandler<CancelInviteCommand, ApiResponse<Invite>>
	{
		public Task<ApiResponse<Invite>> Handle(CancelInviteCommand request, CancellationToken cancellationToken)
		{
			return Task.FromResult(new ApiResponse<Invite>(request.Invite.BeCanceled()));
		}
	}
}