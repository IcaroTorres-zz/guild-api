using Application.Common.Abstractions;
using Application.Common.Results;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Invites.Commands.InviteMember
{
    public class InviteMemberHandler : IRequestHandler<InviteMemberCommand, IApiResult>
    {
        private readonly IUnitOfWork _unit;

        public InviteMemberHandler(IUnitOfWork unit)
        {
            _unit = unit;
        }

        public async Task<IApiResult> Handle(InviteMemberCommand command, CancellationToken cancellationToken)
        {
            var invitingGuild = await _unit.Guilds.GetForMemberHandlingAsync(command.GuildId, cancellationToken);
            var invitedMember = await _unit.Members.GetForGuildOperationsAsync(command.MemberId, cancellationToken);
            var invite = invitingGuild.InviteMember(invitedMember).GetLatestInvite();

            invite = await _unit.Invites.InsertAsync(invite, cancellationToken);

            return new SuccessCreatedResult(invite, command);
        }
    }
}