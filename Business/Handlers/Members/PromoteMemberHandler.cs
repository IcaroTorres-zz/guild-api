using Business.Commands;
using Business.Commands.Members;
using Domain.Entities;
using Domain.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Business.Handlers.Members
{
    public class PromoteMemberHandler : IPipelineBehavior<PromoteMemberCommand, ApiResponse<Member>>
    {
        private readonly IMemberRepository _memberRepository;

        public PromoteMemberHandler(IMemberRepository memberRepository)
        {
            _memberRepository = memberRepository;
        }

        public async Task<ApiResponse<Member>> Handle(PromoteMemberCommand request,
            CancellationToken cancellationToken, RequestHandlerDelegate<ApiResponse<Member>> next)
        {
            return new ApiResponse<Member>(await Task.FromResult(_memberRepository.Update(request.Member.BePromoted())));
        }
    }
}
