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

        public GetGuildHandler(IGuildRepository guildRepository)
        {
            _guildRepository = guildRepository;
        }

        public async Task<IApiResult> Handle(GetGuildCommand command, CancellationToken cancellationToken)
        {
            var result = new ApiResult();
            var guild = await _guildRepository.GetByIdAsync(command.Id, true, cancellationToken);
            return result.SetResult(guild);
        }
    }
}