using System.Threading;
using System.Threading.Tasks;
using Business.Commands.Members;
using Business.ResponseOutputs;
using Domain.Entities;
using Domain.Repositories;
using MediatR;

namespace Business.Handlers.Members
{
	public class DemoteMemberHandler : IPipelineBehavior<DemoteMemberCommand, ApiResponse<Member>>
	{
		private readonly IMemberRepository _memberRepository;

		public DemoteMemberHandler(IMemberRepository memberRepository)
		{
			_memberRepository = memberRepository;
		}

		public async Task<ApiResponse<Member>> Handle(DemoteMemberCommand request, CancellationToken cancellationToken,
			RequestHandlerDelegate<ApiResponse<Member>> next)
		{
			return new ApiResponse<Member>(await Task.FromResult(_memberRepository.Update(request.Member.BeDemoted())));
		}
	}
}