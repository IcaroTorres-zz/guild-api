using AutoMapper;
using Business.Dtos;
using Business.Responses;
using Domain.Responses;
using Domain.Unities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Business.Usecases.Invites.InviteMember
{
    public class InviteMemberHandler : IRequestHandler<InviteMemberCommand, IApiResult>
    {
        private readonly IUnitOfWork _unit;
        private readonly IMapper _mapper;

        public InviteMemberHandler(IUnitOfWork unit, IMapper mapper)
        {
            _unit = unit;
            _mapper = mapper;
        }

        public async Task<IApiResult> Handle(InviteMemberCommand command, CancellationToken cancellationToken)
        {
            var result = new ApiResult();

            var invitingGuild = await _unit.Guilds.GetForMemberHandlingAsync(command.GuildId, cancellationToken);
            var invitedMember = await _unit.Members.GetForGuildOperationsAsync(command.MemberId, cancellationToken);
            var invite = invitingGuild.InviteMember(invitedMember).LatestInvite;

            invite = await _unit.Invites.InsertAsync(invite, cancellationToken);
            var inviteResult = _mapper.Map<InviteDto>(invite);

            return result.SetCreated(inviteResult, command);
        }
    }
}