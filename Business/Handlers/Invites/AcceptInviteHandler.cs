using System.Threading;
using System.Threading.Tasks;
using Business.Commands.Invites;
using Business.ResponseOutputs;
using Domain.Entities;
using MediatR;

namespace Business.Handlers.Invites
{
	public class AcceptInviteHandler : IPipelineBehavior<AcceptInviteCommand, ApiResponse<Invite>>
	{
		public Task<ApiResponse<Invite>> Handle(AcceptInviteCommand request,
			CancellationToken cancellationToken, RequestHandlerDelegate<ApiResponse<Invite>> next)
		{
			return Task.FromResult(new ApiResponse<Invite>(request.Invite.BeAccepted()));
		}
	}
}