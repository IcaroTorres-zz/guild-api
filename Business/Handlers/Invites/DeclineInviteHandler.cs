using System.Threading;
using System.Threading.Tasks;
using Business.Commands.Invites;
using Business.Responses;
using Domain.Entities;
using MediatR;

namespace Business.Handlers.Invites
{
	public class DeclineInviteHandler : IRequestHandler<DeclineInviteCommand, ApiResponse<Invite>>
	{
		public Task<ApiResponse<Invite>> Handle(DeclineInviteCommand request, CancellationToken cancellationToken)
		{
			return Task.FromResult(new ApiResponse<Invite>(request.Invite.BeDeclined()));
		}
	}
}