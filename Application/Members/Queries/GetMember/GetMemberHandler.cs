using Application.Common.Abstractions;
using Application.Common.Results;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Members.Queries.GetMember
{
    public class GetMemberHandler : IRequestHandler<GetMemberCommand, IApiResult>
    {
        private readonly IMemberRepository _memberRepository;

        public GetMemberHandler(IMemberRepository memberRepository)
        {
            _memberRepository = memberRepository;
        }

        public async Task<IApiResult> Handle(GetMemberCommand command, CancellationToken cancellationToken)
        {
            var result = new ApiResult();
            var member = await _memberRepository.GetByIdAsync(command.Id, readOnly: true, cancellationToken);
            return result.SetResult(member);
        }
    }
}