using Application.Common.Abstractions;
using Application.Common.Results;
using Domain.Common;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Members.Commands.CreateMember
{
    public class CreateMemberHandler : IRequestHandler<CreateMemberCommand, IApiResult>
    {
        private readonly IMemberRepository _memberRepository;
        private readonly IModelFactory _factory;

        public CreateMemberHandler(IMemberRepository memberRepository, IModelFactory factory)
        {
            _memberRepository = memberRepository;
            _factory = factory;
        }

        public async Task<IApiResult> Handle(CreateMemberCommand command, CancellationToken cancellationToken)
        {
            var member = _factory.CreateMember(command.Name);
            member = await _memberRepository.InsertAsync(member, cancellationToken);

            return new SuccessCreatedResult(member, command);
        }
    }
}