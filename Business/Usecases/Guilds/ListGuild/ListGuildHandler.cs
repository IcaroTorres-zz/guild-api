using Business.Responses;
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

        public ListGuildHandler(IGuildRepository guildRepository)
        {
            _guildRepository = guildRepository;
        }

        public async Task<IApiResult> Handle(ListGuildCommand command, CancellationToken cancellationToken)
        {
            var result = new ApiResult();

            var pagedGuilds = await _guildRepository.PaginateAsync(
                top: command.PageSize,
                page: command.Page,
                cancellationToken);

            return result.SetResult(pagedGuilds.SetAppliedCommand(command));
        }
    }
}