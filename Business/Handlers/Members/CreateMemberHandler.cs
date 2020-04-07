using System.Threading;
using System.Threading.Tasks;
using Business.Commands.Members;
using Business.Responses;
using Domain.Entities;
using Domain.Repositories;
using MediatR;

namespace Business.Handlers.Members
{
	public class CreateMemberHandler : IRequestHandler<CreateMemberCommand, ApiResponse<Member>>
	{
		private readonly IMemberRepository _memberRepository;

		public CreateMemberHandler(IMemberRepository memberRepository)
		{
			_memberRepository = memberRepository;
		}

		public async Task<ApiResponse<Member>> Handle(CreateMemberCommand request, CancellationToken cancellationToken)
		{
			var createdMember = await _memberRepository.InsertAsync(new Member(request.Name), cancellationToken);
			return new ApiResponse<Member>(createdMember);
		}
	}
}