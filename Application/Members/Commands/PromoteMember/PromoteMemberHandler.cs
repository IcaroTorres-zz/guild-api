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
            var promotionMember = await _memberRepository.GetForGuildOperationsAsync(command.Id, cancellationToken);
            var previousGuildLeader = promotionMember.GetGuild().GetLeader();
            promotionMember = promotionMember.GetGuild().Promote(promotionMember);

            promotionMember = _memberRepository.Update(promotionMember);
            _memberRepository.Update(previousGuildLeader);

            return new SuccessResult(promotionMember);
        }
    }
}