using Business.Responses;
using Domain.Repositories;
using Domain.Responses;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Business.Usecases.Members.ListMember
{
    public class ListMemberHandler : IRequestHandler<ListMemberCommand, IApiResult>
    {
        private readonly IMemberRepository _memberRepository;

        public ListMemberHandler(IMemberRepository memberRepository)
        {
            _memberRepository = memberRepository;
        }

        public async Task<IApiResult> Handle(ListMemberCommand command, CancellationToken cancellationToken)
        {
            var result = new ApiResult();

            var pagedMembers = await _memberRepository.PaginateAsync(
                top: command.PageSize,
                page: command.Page,
                cancellationToken: cancellationToken);

            pagedMembers.SetAppliedCommand(command);
            return result.SetResult(pagedMembers);
        }
    }
}