using Business.Responses;
using Domain.Responses;
using Domain.Unities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Business.Usecases.Invites.AcceptInvite
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
            var result = new ApiResult();

            var invite = await _unit.Invites.GetForAcceptOperationAsync(command.Id, cancellationToken);

            foreach (var inviteToCancel in invite.BeAccepted().InvitesToCancel)
            {
                inviteToCancel.BeCanceled();
                _unit.Invites.Update(inviteToCancel);
            }

            invite = _unit.Invites.Update(invite);
            _unit.Members.Update(invite.Member);
            _unit.Memberships.Update(invite.Member.LastFinishedMembership);
            await _unit.Memberships.InsertAsync(invite.Member.ActiveMembership);

            return result.SetResult(invite);
        }
    }
}