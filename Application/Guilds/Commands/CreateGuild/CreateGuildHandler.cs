using Application.Common.Abstractions;
using Application.Common.Results;
using Domain.Models;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Guilds.Commands.CreateGuild
{
    public class CreateGuildHandler : IRequestHandler<CreateGuildCommand, IApiResult>
    {
        private readonly IUnitOfWork _unit;

        public CreateGuildHandler(IUnitOfWork unit)
        {
            _unit = unit;
        }

        public async Task<IApiResult> Handle(CreateGuildCommand command, CancellationToken cancellationToken)
        {
            var leader = await _unit.Members.GetForGuildOperationsAsync(command.LeaderId, cancellationToken);
            var guild = new Guild(command.Name, leader);

            await Task.WhenAll(
                _unit.Guilds.InsertAsync(guild),
                _unit.Invites.InsertAsync(guild.GetLatestInvite()),
                _unit.Memberships.InsertAsync(leader.GetActiveMembership()));

            _unit.Members.Update(leader);
            _unit.Memberships.Update(leader.GetLastFinishedMembership());

            return new SuccessCreatedResult(guild, command);
        }
    }
}