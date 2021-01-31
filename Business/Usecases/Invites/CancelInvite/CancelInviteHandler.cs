using Business.Responses;
using Domain.Repositories;
using Domain.Responses;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Business.Usecases.Invites.CancelInvite
{
    public class CancelInviteHandler : IRequestHandler<CancelInviteCommand, IApiResult>
    {
        private readonly IInviteRepository _inviteRepository;

        public CancelInviteHandler(IInviteRepository inviteRepository)
        {
            _inviteRepository = inviteRepository;
        }

        public async Task<IApiResult> Handle(CancelInviteCommand command, CancellationToken cancellationToken)
        {
            var result = new ApiResult();

            var invite = await _inviteRepository.GetByIdAsync(command.Id, readOnly: false, cancellationToken);
            invite = _inviteRepository.Update(invite.BeCanceled());

            return result.SetResult(invite);
        }
    }
}