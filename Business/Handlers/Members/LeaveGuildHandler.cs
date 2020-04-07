using System.Threading;
using System.Threading.Tasks;
using Business.Commands.Members;
using Business.Responses;
using Domain.Entities;
using Domain.Repositories;
using MediatR;

namespace Business.Handlers.Members
{
	public class LeaveGuildHandler : IRequestHandler<LeaveGuildCommand, ApiResponse<Member>>
	{
		private readonly IMemberRepository _memberRepository;

		public LeaveGuildHandler(IMemberRepository memberRepository)
		{
			_memberRepository = memberRepository;
		}

		public Task<ApiResponse<Member>> Handle(LeaveGuildCommand request, CancellationToken cancellationToken)
		{
			var leavingMember = _memberRepository.Update(request.Member.LeaveGuild());
			return Task.FromResult(new ApiResponse<Member>(leavingMember));
		}
	}
}