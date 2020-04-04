using System.Threading;
using System.Threading.Tasks;
using Business.Commands.Invites;
using Business.ResponseOutputs;
using Domain.Entities;
using Domain.Repositories;
using MediatR;

namespace Business.Handlers.Invites
{
	public class InviteMemberHandler : IPipelineBehavior<InviteMemberCommand, ApiResponse<Invite>>
	{
		private readonly IInviteRepository _inviteRepository;

		public InviteMemberHandler(IInviteRepository inviteRepository)
		{
			_inviteRepository = inviteRepository;
		}

		public async Task<ApiResponse<Invite>> Handle(InviteMemberCommand request, CancellationToken cancelToken,
			RequestHandlerDelegate<ApiResponse<Invite>> next)
		{
			var invite = new Invite(request.MemberId, request.GuildId);

			return new ApiResponse<Invite>(await _inviteRepository.InsertAsync(invite));
		}
	}
}