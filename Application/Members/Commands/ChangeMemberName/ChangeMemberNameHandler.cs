using Application.Common.Abstractions;
using Application.Common.Results;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Members.Commands.ChangeMemberName
{
    public class ChangeMemberNameHandler : IRequestHandler<ChangeMemberNameCommand, IApiResult>
    {
        private readonly IMemberRepository _memberRepository;

        public ChangeMemberNameHandler(IMemberRepository memberRepository)
        {
            _memberRepository = memberRepository;
        }

        public async Task<IApiResult> Handle(ChangeMemberNameCommand command, CancellationToken cancellationToken)
        {
            var member = await _memberRepository.GetByIdAsync(command.Id);
            member = _memberRepository.Update(member.ChangeName(command.Name));

            return new SuccessResult(member);
        }
    }
}