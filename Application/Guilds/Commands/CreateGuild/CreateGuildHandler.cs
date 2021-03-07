using Application.Common.Abstractions;
using Application.Common.Results;
using Domain.Common;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Guilds.Commands.CreateGuild
{
    public class CreateGuildHandler : IRequestHandler<CreateGuildCommand, IApiResult>
    {
        private readonly IUnitOfWork _unit;
        private readonly IModelFactory _factory;

        public CreateGuildHandler(IUnitOfWork unit, IModelFactory factory)
        {
            _unit = unit;
            _factory = factory;
        }

        public async Task<IApiResult> Handle(CreateGuildCommand command, CancellationToken cancellationToken)
        {
            var leader = await _unit.Members.GetForGuildOperationsAsync(command.LeaderId, cancellationToken);
            var previousMembership = leader.GetActiveMembership();
            var newGuild = _factory.CreateGuild(command.Name, leader);
            var newInvite = newGuild.GetLatestInvite();
            var newMembership = leader.GetActiveMembership();

            var guildtask = _unit.Guilds.InsertAsync(newGuild, cancellationToken);
            var invitetask = _unit.Invites.InsertAsync(newInvite, cancellationToken);
            var membershiptask = _unit.Memberships.InsertAsync(newMembership, cancellationToken);

            _unit.Members.Update(leader);
            _unit.Memberships.Update(previousMembership);

            await invitetask;
            await membershiptask;
            newGuild = await guildtask;

            return new SuccessCreatedResult(newGuild, command);
        }
    }
}