using Business.Commands;
using Business.Commands.Members;
using Domain.Entities;
using Domain.Repositories;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Business.Handlers.Members
{
    public class UpdateMemberHandler : IPipelineBehavior<UpdateMemberCommand, ApiResponse<Member>>
    {
        private readonly IMemberRepository _memberRepository;
        private readonly IGuildRepository _guildRepository;

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
            {
                (await _guildRepository.GetForMemberHandlingAsync(guildId))
                    .Invite(member)
                    .BeAccepted();
            }
            else
            {
                member.LeaveGuild();
            }
            return new ApiResponse<Member>(_memberRepository.Update(member));
        }
    }
}
