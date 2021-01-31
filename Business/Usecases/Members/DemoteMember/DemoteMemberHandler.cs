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

        public DemoteMemberHandler(IMemberRepository memberRepository)
        {
            _memberRepository = memberRepository;
        }

        public async Task<IApiResult> Handle(DemoteMemberCommand command, CancellationToken cancellationToken)
        {
            var result = new ApiResult();

            var masterToDemote = await _memberRepository.GetForGuildOperationsAsync(command.Id, cancellationToken);
            var guild = masterToDemote.Guild.DemoteLeader();
            var promotedNewLeader = guild.Leader;

            masterToDemote = _memberRepository.Update(masterToDemote);
            _memberRepository.Update(promotedNewLeader);

            return result.SetResult(masterToDemote);
        }
    }
}