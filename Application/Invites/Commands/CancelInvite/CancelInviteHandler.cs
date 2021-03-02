using Application.Common.Abstractions;
using Application.Common.Results;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Invites.Commands.CancelInvite
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
            var invite = await _inviteRepository.GetByIdAsync(command.Id, readOnly: false, cancellationToken);
            invite = _inviteRepository.Update(invite.BeCanceled());

            return new SuccessResult(invite);
        }
    }
}