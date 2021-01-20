using AutoMapper;
using Business.Dtos;
using Business.Responses;
using Domain.Repositories;
using Domain.Responses;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Business.Usecases.Members.DemoteMember
{
    public class DemoteMemberHandler : IRequestHandler<DemoteMemberCommand, IApiResult>
    {
        private readonly IMemberRepository _memberRepository;
        private readonly IMapper _mapper;

        public DemoteMemberHandler(IMemberRepository memberRepository, IMapper mapper)
        {
            _memberRepository = memberRepository;
            _mapper = mapper;
        }

        public async Task<IApiResult> Handle(DemoteMemberCommand command, CancellationToken cancellationToken)
        {
            var result = new ApiResult();

            var masterToDemote = await _memberRepository.GetForGuildOperationsAsync(command.Id, cancellationToken);
            var guild = masterToDemote.Guild.DemoteLeader();
            var promotedNewLeader = guild.Leader;

            masterToDemote = _memberRepository.Update(masterToDemote);
            _memberRepository.Update(promotedNewLeader);

            var updateResult = _mapper.Map<MemberDto>(masterToDemote);

            return result.SetResult(updateResult);
        }
    }
}