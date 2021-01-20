using AutoMapper;
using Business.Dtos;
using Business.Responses;
using Domain.Repositories;
using Domain.Responses;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Business.Usecases.Guilds.GetGuild
{
    public class GetGuildHandler : IRequestHandler<GetGuildCommand, IApiResult>
    {
        private readonly IGuildRepository _guildRepository;
        private readonly IMapper _mapper;

        public GetGuildHandler(IGuildRepository guildRepository, IMapper mapper)
        {
            _guildRepository = guildRepository;
            _mapper = mapper;
        }

        public async Task<IApiResult> Handle(GetGuildCommand command, CancellationToken cancellationToken)
        {
            var result = new ApiResult();
            var guild = await _guildRepository.GetByIdAsync(command.Id, true, cancellationToken);
            return result.SetResult(_mapper.Map<GuildDto>(guild));
        }
    }
}