using Application.Common.Abstractions;
using Application.Common.Results;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Invites.Commands.AcceptInvite
{
    public class AcceptInviteHandler : IRequestHandler<AcceptInviteCommand, IApiResult>
    {
        private readonly IUnitOfWork _unit;

        public AcceptInviteHandler(IUnitOfWork unit)
        {
            _unit = unit;
        }

        public async Task<IApiResult> Handle(AcceptInviteCommand command, CancellationToken cancellationToken)
        {
            var invite = await _unit.Invites.GetForAcceptOperationAsync(command.Id, cancellationToken);

            foreach (var inviteToCancel in invite.BeAccepted().GetInvitesToCancel())
            {
                inviteToCancel.BeCanceled();
                _unit.Invites.Update(inviteToCancel);
            }

            invite = _unit.Invites.Update(invite);
            _unit.Members.Update(invite.Member);
            _unit.Memberships.Update(invite.Member.GetLastFinishedMembership());
            await _unit.Memberships.InsertAsync(invite.Member.GetActiveMembership());

            return new SuccessResult(invite);
        }
    }
}