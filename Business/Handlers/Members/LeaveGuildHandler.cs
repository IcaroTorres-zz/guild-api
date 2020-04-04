using System.Threading;
using System.Threading.Tasks;
using Business.Commands.Members;
using Business.ResponseOutputs;
using Domain.Entities;
using Domain.Repositories;
using MediatR;

namespace Business.Handlers.Members
{
	public class LeaveGuildHandler : IPipelineBehavior<LeaveGuildCommand, ApiResponse<Member>>
	{
		private readonly IMemberRepository _memberRepository;

		public LeaveGuildHandler(IMemberRepository memberRepository)
		{
			_memberRepository = memberRepository;
		}

		public async Task<ApiResponse<Member>> Handle(LeaveGuildCommand request,
			CancellationToken cancellationToken, RequestHandlerDelegate<ApiResponse<Member>> next)
		{
			return new ApiResponse<Member>(
				await Task.FromResult(_memberRepository.Update(request.Member.LeaveGuild())));
		}
	}
}