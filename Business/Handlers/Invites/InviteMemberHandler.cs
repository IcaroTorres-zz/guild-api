using System.Threading;
using System.Threading.Tasks;
using Business.Commands.Invites;
using Business.Responses;
using Domain.Entities;
using Domain.Repositories;
using MediatR;

namespace Business.Handlers.Invites
{
	public class InviteMemberHandler : IRequestHandler<InviteMemberCommand, ApiResponse<Invite>>
	{
		private readonly IInviteRepository _inviteRepository;

		public InviteMemberHandler(IInviteRepository inviteRepository)
		{
			_inviteRepository = inviteRepository;
		}

		public async Task<ApiResponse<Invite>> Handle(InviteMemberCommand request, CancellationToken cancellationToken)
		{
			var invite = new Invite(request.MemberId, request.GuildId);
			return new ApiResponse<Invite>(await _inviteRepository.InsertAsync(invite, cancellationToken));
		}
	}
}