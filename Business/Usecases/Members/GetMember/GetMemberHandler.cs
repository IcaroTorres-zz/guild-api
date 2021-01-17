using AutoMapper;
using Business.Dtos;
using Business.Responses;
using Domain.Repositories;
using Domain.Responses;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Business.Usecases.Members.GetMember
{
    public class GetMemberHandler : IRequestHandler<GetMemberCommand, IApiResult>
    {
        private readonly IMemberRepository _memberRepository;
        private readonly IMapper _mapper;

        public GetMemberHandler(IMemberRepository memberRepository, IMapper mapper)
        {
            _memberRepository = memberRepository;
            _mapper = mapper;
        }

        public async Task<IApiResult> Handle(GetMemberCommand command, CancellationToken cancellationToken)
        {
            var response = new ApiResult();
            var member = await _memberRepository.GetByIdAsync(command.Id, readOnly: true, cancellationToken);
            return response.SetResult(_mapper.Map<MemberDto>(member));
        }
    }
}