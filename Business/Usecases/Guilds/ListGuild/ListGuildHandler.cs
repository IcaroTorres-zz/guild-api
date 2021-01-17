using AutoMapper;
using Business.Dtos;
using Business.Responses;
using Domain.Models;
using Domain.Repositories;
using Domain.Responses;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Business.Usecases.Guilds.ListGuild
{
    public class ListGuildHandler : IRequestHandler<ListGuildCommand, IApiResult>
    {
        private readonly IGuildRepository _guildRepository;
        private readonly IMapper _mapper;

        public ListGuildHandler(IGuildRepository guildRepository, IMapper mapper)
        {
            _guildRepository = guildRepository;
            _mapper = mapper;
        }

        public async Task<IApiResult> Handle(ListGuildCommand command, CancellationToken cancellationToken)
        {
            var response = new ApiResult();

            var pagedGuilds = await _guildRepository.PaginateAsync(
                top: command.PageSize,
                page: command.Page,
                cancellationToken);

            var pagedResult = _mapper.Map<Pagination<GuildDto>>(pagedGuilds);
            pagedResult.SetAppliedCommand(command);
            return response.SetResult(pagedResult);
        }
    }
}