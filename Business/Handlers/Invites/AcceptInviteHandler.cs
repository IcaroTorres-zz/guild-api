using System.Threading;
using System.Threading.Tasks;
using Business.Commands.Invites;
using Business.Responses;
using Domain.Entities;
using MediatR;

namespace Business.Handlers.Invites
{
	public class AcceptInviteHandler : IRequestHandler<AcceptInviteCommand, ApiResponse<Invite>>
	{
		public Task<ApiResponse<Invite>> Handle(AcceptInviteCommand request, CancellationToken cancellationToken)
		{
			return Task.FromResult(new ApiResponse<Invite>(request.Invite.BeAccepted()));
		}
	}
}