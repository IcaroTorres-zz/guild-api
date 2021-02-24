using Application.Common.Abstractions;
using Application.Common.Results;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Members.Commands.PromoteMember
{
    public class PromoteMemberHandler : IRequestHandler<PromoteMemberCommand, IApiResult>
    {
        private readonly IMemberRepository _memberRepository;

        public PromoteMemberHandler(IMemberRepository memberRepository)
        {
            _memberRepository = memberRepository;
        }

        public async Task<IApiResult> Handle(PromoteMemberCommand command, CancellationToken cancellationToken)
        {
            var result = new ApiResult();

            var promotionMember = await _memberRepository.GetForGuildOperationsAsync(command.Id, cancellationToken);
            var guild = promotionMember.Guild;
            var previousGuildLeader = guild.GetLeader();

            guild.Promote(promotionMember);
            promotionMember = _memberRepository.Update(promotionMember);
            _memberRepository.Update(previousGuildLeader);

            return result.SetResult(promotionMember);
        }
    }
}