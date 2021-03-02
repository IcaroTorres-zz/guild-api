using Application.Common.Abstractions;
using Application.Common.Results;
using Domain.Models;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Members.Commands.CreateMember
{
    public class CreateMemberHandler : IRequestHandler<CreateMemberCommand, IApiResult>
    {
        private readonly IMemberRepository _memberRepository;

        public CreateMemberHandler(IMemberRepository memberRepository)
        {
            _memberRepository = memberRepository;
        }

        public async Task<IApiResult> Handle(CreateMemberCommand command, CancellationToken cancellationToken)
        {
            var member = new Member(command.Name);
            member = await _memberRepository.InsertAsync(member, cancellationToken);

            return new SuccessCreatedResult(member, command);
        }
    }
}