using Business.Commands.Invites;
using Business.ResponseOutputs;
using Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Business.Handlers.Invites
{
  public class DeclineInviteHandler : IPipelineBehavior<DeclineInviteCommand, ApiResponse<Invite>>
  {
    public Task<ApiResponse<Invite>> Handle(DeclineInviteCommand request,
        CancellationToken cancellationToken, RequestHandlerDelegate<ApiResponse<Invite>> next)
    {
      return Task.FromResult(new ApiResponse<Invite>(request.Invite.BeDeclined()));
    }
  }
}
