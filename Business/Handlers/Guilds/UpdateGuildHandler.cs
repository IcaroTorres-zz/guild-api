using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Business.Commands.Guilds;
using Business.ResponseOutputs;
using Domain.Entities;
using Domain.Repositories;
using MediatR;

namespace Business.Handlers.Guilds
{
	public class UpdateGuildHandler : IPipelineBehavior<UpdateGuildCommand, ApiResponse<Guild>>
	{
		private readonly IGuildRepository _guildRepository;
		private readonly IMemberRepository _memberRepository;

		public UpdateGuildHandler(IGuildRepository guildRepository, IMemberRepository memberRepository)
		{
			_guildRepository = guildRepository;
			_memberRepository = memberRepository;
		}

		public async Task<ApiResponse<Guild>> Handle(UpdateGuildCommand request,
			CancellationToken cancellationToken, RequestHandlerDelegate<ApiResponse<Guild>> next)
		{
			var guild = await _guildRepository.GetForMemberHandlingAsync(request.Id);
			var master = await _memberRepository.GetForGuildOperationsAsync(request.MasterId);

			var currentMemberIds = guild.Members.Select(x => x.Id);
			var receivedAlreadyMemberIds = request.MemberIds.Intersect(currentMemberIds);
			var idsToInvite = request.MemberIds.Except(receivedAlreadyMemberIds);
			var idsToKick = currentMemberIds.Except(receivedAlreadyMemberIds);

			_memberRepository.Query(x => !x.Disabled)
				.Join(idsToInvite, x => x.Id, id => id, (member, _) => member).ToList()
				.ForEach(memberToInvite => guild.Invite(memberToInvite)?.BeAccepted());

			guild.Members
				.Join(idsToKick, x => x.Id, id => id, (member, _) => member).ToList()
				.ForEach(memberToKick => memberToKick.LeaveGuild());

			guild.ChangeName(request.Name);
			guild.Promote(master);
			return new ApiResponse<Guild>(_guildRepository.Update(guild));
		}
	}
}