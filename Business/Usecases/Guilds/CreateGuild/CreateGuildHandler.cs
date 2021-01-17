using AutoMapper;
using Business.Dtos;
using Business.Responses;
using Domain.Models;
using Domain.Responses;
using Domain.Unities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Business.Usecases.Guilds.CreateGuild
{
    public class CreateGuildHandler : IRequestHandler<CreateGuildCommand, IApiResult>
    {
        private readonly IUnitOfWork _unit;
        private readonly IMapper _mapper;

        public CreateGuildHandler(IUnitOfWork unit, IMapper mapper)
        {
            _unit = unit;
            _mapper = mapper;
        }

        public async Task<IApiResult> Handle(CreateGuildCommand command, CancellationToken cancellationToken)
        {
            var response = new ApiResult();

            var master = await _unit.Members.GetForGuildOperationsAsync(command.MasterId, cancellationToken);
            var guild = new Guild(command.Name, master);

            await Task.WhenAll(
                _unit.Guilds.InsertAsync(guild),
                _unit.Invites.InsertAsync(guild.LatestInvite),
                _unit.Memberships.InsertAsync(master.ActiveMembership));

            _unit.Members.Update(master);
            _unit.Memberships.Update(master.LastFinishedMembership);

            var creationResult = _mapper.Map<GuildDto>(guild);

            return response.SetCreated(creationResult, command);
        }
    }
}