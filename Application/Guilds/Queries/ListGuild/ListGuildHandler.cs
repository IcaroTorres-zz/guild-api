using Application.Common.Abstractions;
using Application.Common.Results;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Guilds.Queries.ListGuild
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
            var pagedGuilds = await _guildRepository.PaginateAsync(
                top: command.PageSize,
                page: command.Page,
                cancellationToken);

            return new SuccessResult(pagedGuilds.SetAppliedCommand(command));
        }
    }
}