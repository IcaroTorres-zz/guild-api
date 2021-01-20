using AutoMapper;
using Business.Dtos;
using Business.Responses;
using Domain.Models;
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
        private readonly IMapper _mapper;

        public ListMemberHandler(IMemberRepository memberRepository, IMapper mapper)
        {
            _memberRepository = memberRepository;
            _mapper = mapper;
        }

        public async Task<IApiResult> Handle(ListMemberCommand command, CancellationToken cancellationToken)
        {
            var result = new ApiResult();

            var pagedMembers = await _memberRepository.PaginateAsync(
                top: command.PageSize,
                page: command.Page,
                cancellationToken: cancellationToken);

            var pagedResult = _mapper.Map<Pagination<MemberDto>>(pagedMembers);
            pagedResult.SetAppliedCommand(command);
            return result.SetResult(pagedResult);
        }
    }
}