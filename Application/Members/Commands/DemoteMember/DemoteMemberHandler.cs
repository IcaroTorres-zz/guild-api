using Application.Common.Abstractions;
using Application.Common.Results;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Members.Commands.DemoteMember
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
            var masterToDemote = await _memberRepository.GetForGuildOperationsAsync(command.Id, cancellationToken);
            var promotedAfterDemotion = masterToDemote.GetGuild().DemoteLeader();
            masterToDemote = _memberRepository.Update(masterToDemote);
            _memberRepository.Update(promotedAfterDemotion);

            return new SuccessResult(masterToDemote);
        }
    }
}