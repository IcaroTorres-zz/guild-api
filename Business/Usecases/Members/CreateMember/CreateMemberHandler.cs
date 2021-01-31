using Business.Responses;
using Domain.Models;
using Domain.Repositories;
using Domain.Responses;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Business.Usecases.Members.CreateMember
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
            var result = new ApiResult();

            var member = new Member(command.Name);
            member = await _memberRepository.InsertAsync(member, cancellationToken);

            return result.SetCreated(member, command);
        }
    }
}