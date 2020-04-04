using System;
using System.Threading;
using System.Threading.Tasks;
using Business.Commands.Members;
using Business.ResponseOutputs;
using Domain.Entities;
using Domain.Repositories;
using MediatR;

namespace Business.Handlers.Members
{
	public class UpdateMemberHandler : IPipelineBehavior<UpdateMemberCommand, ApiResponse<Member>>
	{
		private readonly IGuildRepository _guildRepository;
		private readonly IMemberRepository _memberRepository;

		public UpdateMemberHandler(IGuildRepository guildRepository, IMemberRepository memberRepository)
		{
			_guildRepository = guildRepository;
			_memberRepository = memberRepository;
		}

		public async Task<ApiResponse<Member>> Handle(UpdateMemberCommand request,
			CancellationToken cancellationToken, RequestHandlerDelegate<ApiResponse<Member>> next)
		{
			var member = await _memberRepository.GetForGuildOperationsAsync(request.Id);
			member.ChangeName(request.Name);
			if (request.GuildId is Guid guildId && guildId != Guid.Empty)
				(await _guildRepository.GetForMemberHandlingAsync(guildId))
					.Invite(member)
					.BeAccepted();
			else
				member.LeaveGuild();
			return new ApiResponse<Member>(_memberRepository.Update(member));
		}
	}
}