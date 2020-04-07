using System;
using System.Threading;
using System.Threading.Tasks;
using Business.Commands.Members;
using Business.Responses;
using Domain.Entities;
using Domain.Repositories;
using MediatR;

namespace Business.Handlers.Members
{
	public class UpdateMemberHandler : IRequestHandler<UpdateMemberCommand, ApiResponse<Member>>
	{
		private readonly IGuildRepository _guildRepository;
		private readonly IMemberRepository _memberRepository;

		public UpdateMemberHandler(IGuildRepository guildRepository, IMemberRepository memberRepository)
		{
			_guildRepository = guildRepository;
			_memberRepository = memberRepository;
		}

		public async Task<ApiResponse<Member>> Handle(UpdateMemberCommand request, CancellationToken cancellationToken)
		{
			var updatedMember = await _memberRepository.GetForGuildOperationsAsync(request.Id, cancellationToken);
			updatedMember.ChangeName(request.Name);
			if (request.GuildId is { } guildId && guildId != Guid.Empty)
			{
				var invitingGuild = await _guildRepository.GetForMemberHandlingAsync(guildId, cancellationToken);
				invitingGuild.Invite(updatedMember).BeAccepted();
			}
			else updatedMember.LeaveGuild();

			return new ApiResponse<Member>(_memberRepository.Update(updatedMember));
		}
	}
}