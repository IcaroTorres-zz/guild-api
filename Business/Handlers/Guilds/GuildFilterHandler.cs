using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Business.Commands.Guilds;
using Business.ResponseOutputs;
using Domain.Entities;
using Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

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
			var query = _guildRepository.Query(x => !x.Disabled, true);
			var totalCount = query.Count();
			var guilds = await query.Take((int) request.Count).ToListAsync(cancellationToken);

			return new ApiResponse<Pagination<Guild>>(new Pagination<Guild>(guilds, totalCount, (int) request.Count));
		}
	}
}