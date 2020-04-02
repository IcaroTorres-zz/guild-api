using Business.Commands;
using Business.Commands.Invites;
using Domain.Entities;
using Domain.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Business.Handlers.Invites
{
    public class InviteMemberHandler : IPipelineBehavior<InviteMemberCommand, ApiResponse<Invite>>
    {
        private readonly IInviteRepository _inviteRepository;

        public InviteMemberHandler(IInviteRepository inviteRepository)
        {
            _inviteRepository = inviteRepository;
        }

        public async Task<ApiResponse<Invite>> Handle(InviteMemberCommand request,
            CancellationToken cancellationToken, RequestHandlerDelegate<ApiResponse<Invite>> next)
        {
            return new ApiResponse<Invite>(await _inviteRepository.InsertAsync(request.Invite));
        }
    }
}
