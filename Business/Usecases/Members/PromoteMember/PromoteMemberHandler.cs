using AutoMapper;
using Business.Dtos;
using Business.Responses;
using Domain.Repositories;
using Domain.Responses;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Business.Usecases.Members.PromoteMember
{
    public class PromoteMemberHandler : IRequestHandler<PromoteMemberCommand, IApiResult>
    {
        private readonly IMemberRepository _memberRepository;
        private readonly IMapper _mapper;

        public PromoteMemberHandler(IMemberRepository memberRepository, IMapper mapper)
        {
            _memberRepository = memberRepository;
            _mapper = mapper;
        }

        public async Task<IApiResult> Handle(PromoteMemberCommand command, CancellationToken cancellationToken)
        {
            var response = new ApiResult();

            var promotionMember = await _memberRepository.GetForGuildOperationsAsync(command.Id, cancellationToken);
            var guild = promotionMember.Guild;
            var previousGuildLeader = guild.Leader;

            guild.Promote(promotionMember);
            _memberRepository.Update(promotionMember);
            _memberRepository.Update(previousGuildLeader);

            var updateResult = _mapper.Map<MemberDto>(promotionMember);

            return response.SetResult(updateResult);
        }
    }
}