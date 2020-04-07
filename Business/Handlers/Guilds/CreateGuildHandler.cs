using System.Threading;
using System.Threading.Tasks;
using Business.Commands.Guilds;
using Business.Responses;
using Domain.Entities;
using Domain.Repositories;
using MediatR;

namespace Business.Handlers.Guilds
{
	public class CreateGuildHandler : IRequestHandler<CreateGuildCommand, ApiResponse<Guild>>
	{
		private readonly IGuildRepository _guildRepository;
		private readonly IMemberRepository _memberRepository;

		public CreateGuildHandler(IGuildRepository guildRepository, IMemberRepository memberRepository)
		{
			_guildRepository = guildRepository;
			_memberRepository = memberRepository;
		}

		public async Task<ApiResponse<Guild>> Handle(CreateGuildCommand request, CancellationToken cancellationToken)
		{
			var master = await _memberRepository.GetForGuildOperationsAsync(request.MasterId, cancellationToken);
			var createdGuild = await _guildRepository.InsertAsync(new Guild(request.Name, master), cancellationToken);
			return new ApiResponse<Guild>(createdGuild);
		}
	}
}