using Business.Responses;
using Domain.Repositories;
using Domain.Responses;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Business.Usecases.Invites.DenyInvite
{
    public class DenyInviteHandler : IRequestHandler<DenyInviteCommand, IApiResult>
    {
        private readonly IInviteRepository _inviteRepository;

        public DenyInviteHandler(IInviteRepository inviteRepository)
        {
            _inviteRepository = inviteRepository;
        }

        public async Task<IApiResult> Handle(DenyInviteCommand command, CancellationToken cancellationToken)
        {
            var result = new ApiResult();

            var invite = await _inviteRepository.GetByIdAsync(command.Id, readOnly: false, cancellationToken);
            invite = _inviteRepository.Update(invite.BeDenied());

            return result.SetResult(invite);
        }
    }
}