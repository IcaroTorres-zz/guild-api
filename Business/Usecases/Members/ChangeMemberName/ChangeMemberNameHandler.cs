using Business.Responses;
using Domain.Repositories;
using Domain.Responses;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Business.Usecases.Members.ChangeMemberName
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
            var result = new ApiResult();

            var member = await _memberRepository.GetByIdAsync(command.Id);
            member = _memberRepository.Update(member.ChangeName(command.Name));

            return result.SetResult(member);
        }
    }
}