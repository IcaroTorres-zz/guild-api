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
	public class UpdateGuildHandler : IRequestHandler<UpdateGuildCommand, ApiResponse<Guild>>
	{
		private readonly IGuildRepository _guildRepository;
		private readonly IMemberRepository _memberRepository;

		public UpdateGuildHandler(IGuildRepository guildRepository, IMemberRepository memberRepository)
		{
			_guildRepository = guildRepository;
			_memberRepository = memberRepository;
		}

		public async Task<ApiResponse<Guild>> Handle(UpdateGuildCommand request,
			CancellationToken cancellationToken)
		{
			var guild = await _guildRepository.GetForMemberHandlingAsync(request.Id, cancellationToken);
			var master = await _memberRepository.GetForGuildOperationsAsync(request.MasterId, cancellationToken);

			var currentMemberIds = guild.Members.Select(x => x.Id).ToArray();
			var receivedAlreadyMemberIds = request.MemberIds.Intersect(currentMemberIds).ToArray();
			var idsToInvite = request.MemberIds.Except(receivedAlreadyMemberIds).ToArray();
			var idsToKick = currentMemberIds.Except(receivedAlreadyMemberIds).ToArray();
			
			// invite and accept new members
			await _memberRepository.Query(x => !x.Disabled)
				.Join(idsToInvite, x => x.Id, id => id, (member, _) => member).AsQueryable()
				.ForEachAsync(memberToInvite => guild.Invite(memberToInvite)?.BeAccepted(), cancellationToken);
			
			// kick members not received from the request
			await guild.Members
				.Join(idsToKick, x => x.Id, id => id, (member, _) => member).AsQueryable()
				.ForEachAsync(memberToKick => memberToKick.LeaveGuild(), cancellationToken);

			guild.ChangeName(request.Name);
			guild.Promote(master);
			return new ApiResponse<Guild>(_guildRepository.Update(guild));
		}
	}
}