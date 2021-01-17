using AutoMapper;
using Business.Dtos;
using Business.Responses;
using Domain.Repositories;
using Domain.Responses;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Business.Usecases.Members.ChangeMemberName
{
    public class ChangeMemberNameHandler : IRequestHandler<ChangeMemberNameCommand, IApiResult>
    {
        private readonly IMemberRepository _memberRepository;
        private readonly IMapper _mapper;

        public ChangeMemberNameHandler(IMemberRepository memberRepository, IMapper mapper)
        {
            _memberRepository = memberRepository;
            _mapper = mapper;
        }

        public async Task<IApiResult> Handle(ChangeMemberNameCommand command, CancellationToken cancellationToken)
        {
            var response = new ApiResult();

            var member = await _memberRepository.GetByIdAsync(command.Id);
            member = _memberRepository.Update(member.ChangeName(command.Name));

            var updateResult = _mapper.Map<MemberDto>(member);

            return response.SetResult(updateResult);
        }
    }
}