using Business.Commands;
using Business.Commands.Invites;
using Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

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
