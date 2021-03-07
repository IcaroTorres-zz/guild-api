using Application.Common.Abstractions;
using Application.Common.Results;
using Domain.Common;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Invites.Commands.AcceptInvite
{
    public class AcceptInviteHandler : IRequestHandler<AcceptInviteCommand, IApiResult>
    {
        private readonly IUnitOfWork _unit;
        private readonly IModelFactory _factory;

        public AcceptInviteHandler(IUnitOfWork unit, IModelFactory factory)
        {
            _unit = unit;
            _factory = factory;
        }

        public async Task<IApiResult> Handle(AcceptInviteCommand command, CancellationToken cancellationToken)
        {
            var invite = await _unit.Invites.GetForAcceptOperationAsync(command.Id, cancellationToken);
            var invitedMember = invite.GetMember();
            var previousMembership = invitedMember.GetActiveMembership();
            var possiblePromoted = invitedMember.GetGuild().GetVice();

            var newMembership = invite.BeAccepted(_factory);

            foreach (var inviteToCancel in invite.GetInvitesToCancel())
            {
                inviteToCancel.BeCanceled();
                _unit.Invites.Update(inviteToCancel);
            }

            invite = _unit.Invites.Update(invite);
            _unit.Members.Update(invitedMember);
            _unit.Members.Update(possiblePromoted);
            _unit.Memberships.Update(previousMembership);
            await _unit.Memberships.InsertAsync(newMembership, cancellationToken);

            return new SuccessResult(invite);
        }
    }
}