using Business.Commands;
using Business.Commands.Guilds;
using Domain.Entities;
using Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Business.Handlers.Guilds
{
    public class GuildFilterHandler : IPipelineBehavior<GuildFilterCommand, ApiResponse<Pagination<Guild>>>
    {
        private readonly IGuildRepository _guildRepository;

        public GuildFilterHandler(IGuildRepository guildRepository)
        {
            _guildRepository = guildRepository;
        }

        public async Task<ApiResponse<Pagination<Guild>>> Handle(GuildFilterCommand request,
            CancellationToken cancellationToken, RequestHandlerDelegate<ApiResponse<Pagination<Guild>>> next)
        {
            var query = _guildRepository.Query(x => !x.Disabled, readOnly: true);
            var totalCount = query.Count();
            var guilds = await query.Take((int)request.Count).ToListAsync();

            return new ApiResponse<Pagination<Guild>>(new Pagination<Guild>(guilds, totalCount, (int)request.Count));
        }
    }
}
