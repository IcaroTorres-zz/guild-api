using Application.Common.Abstractions;
using Application.Common.Results;
using Domain.Common;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Invites.Commands.InviteMember
{
    public class InviteMemberHandler : IRequestHandler<InviteMemberCommand, IApiResult>
    {
        private readonly IUnitOfWork _unit;
        private readonly IModelFactory _factory;

        public InviteMemberHandler(IUnitOfWork unit, IModelFactory factory)
        {
            _unit = unit;
            _factory = factory;
        }

        public async Task<IApiResult> Handle(InviteMemberCommand command, CancellationToken cancellationToken)
        {
            var invitingGuild = await _unit.Guilds.GetByIdAsync(command.GuildId, false, cancellationToken);
            var invitedMember = await _unit.Members.GetByIdAsync(command.MemberId, false, cancellationToken);
            var invite = _factory.CreateInvite(invitingGuild, invitedMember);
            invite = await _unit.Invites.InsertAsync(invite, cancellationToken);

            return new SuccessCreatedResult(invite, command);
        }
    }
}