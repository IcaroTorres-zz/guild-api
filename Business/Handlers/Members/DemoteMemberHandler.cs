using System.Threading;
using System.Threading.Tasks;
using Business.Commands.Members;
using Business.Responses;
using Domain.Entities;
using Domain.Repositories;
using MediatR;

namespace Business.Handlers.Members
{
	public class DemoteMemberHandler : IRequestHandler<DemoteMemberCommand, ApiResponse<Member>>
	{
		private readonly IMemberRepository _memberRepository;

		public DemoteMemberHandler(IMemberRepository memberRepository)
		{
			_memberRepository = memberRepository;
		}

		public Task<ApiResponse<Member>> Handle(DemoteMemberCommand request, CancellationToken cancellationToken)
		{
			var demotedMember = _memberRepository.Update(request.Member.BeDemoted());
			return Task.FromResult(new ApiResponse<Member>(demotedMember));
		}
	}
}