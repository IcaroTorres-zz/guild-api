using Business.Responses;
using Domain.Repositories;
using Domain.Responses;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Business.Usecases.Members.LeaveGuild
{
    public class LeaveGuildHandler : IRequestHandler<LeaveGuildCommand, IApiResult>
    {
        private readonly IMemberRepository _memberRepository;
        private readonly IMembershipRepository _membershipRepository;

        public LeaveGuildHandler(IMemberRepository memberRepository, IMembershipRepository membershipRepository)
        {
            _memberRepository = memberRepository;
            _membershipRepository = membershipRepository;
        }

        public async Task<IApiResult> Handle(LeaveGuildCommand command, CancellationToken cancellationToken)
        {
            var result = new ApiResult();

            var leavingMember = await _memberRepository.GetForGuildOperationsAsync(command.Id, cancellationToken);
            leavingMember.LeaveGuild();
            var promotedNewLeader = leavingMember.Guild.Leader;

            leavingMember = _memberRepository.Update(leavingMember);
            _memberRepository.Update(promotedNewLeader);
            _membershipRepository.Update(leavingMember.LastFinishedMembership);

            return result.SetResult(leavingMember);
        }
    }
}