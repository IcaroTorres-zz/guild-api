using Application.Common.Abstractions;
using Application.Common.Results;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Invites.Commands.DenyInvite
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
            var invite = await _inviteRepository.GetByIdAsync(command.Id, readOnly: false, cancellationToken);
            invite = _inviteRepository.Update(invite.BeDenied());

            return new SuccessResult(invite);
        }
    }
}