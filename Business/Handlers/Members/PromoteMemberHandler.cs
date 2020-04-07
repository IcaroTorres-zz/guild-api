using System.Threading;
using System.Threading.Tasks;
using Business.Commands.Members;
using Business.Responses;
using Domain.Entities;
using Domain.Repositories;
using MediatR;

namespace Business.Handlers.Members
{
	public class PromoteMemberHandler : IRequestHandler<PromoteMemberCommand, ApiResponse<Member>>
	{
		private readonly IMemberRepository _memberRepository;

		public PromoteMemberHandler(IMemberRepository memberRepository)
		{
			_memberRepository = memberRepository;
		}

		public Task<ApiResponse<Member>> Handle(PromoteMemberCommand request, CancellationToken cancellationToken)
		{
			var promotedMember = _memberRepository.Update(request.Member.BePromoted());
			return Task.FromResult(new ApiResponse<Member>(promotedMember));
		}
	}
}