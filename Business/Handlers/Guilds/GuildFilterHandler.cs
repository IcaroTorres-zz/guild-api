using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Business.Commands.Guilds;
using Business.Responses;
using Domain.Entities;
using Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Business.Handlers.Guilds
{
	public class GuildFilterHandler : IRequestHandler<GuildFilterCommand, ApiResponse<Pagination<Guild>>>
	{
		private readonly IGuildRepository _guildRepository;

		public GuildFilterHandler(IGuildRepository guildRepository)
		{
			_guildRepository = guildRepository;
		}

		public async Task<ApiResponse<Pagination<Guild>>> Handle(GuildFilterCommand request,
			CancellationToken cancellationToken)
		{
			var query = _guildRepository.Query(readOnly: true);
			var totalCount = query.Count();
			var guilds = await query.Take((int) request.Count).ToListAsync(cancellationToken);
			var guildsPaginated = new Pagination<Guild>(guilds, totalCount, (int) request.Count);
			return new ApiResponse<Pagination<Guild>>(guildsPaginated);
		}
	}
}