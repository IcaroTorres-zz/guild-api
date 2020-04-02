using Business.Commands;
using Business.Commands.Members;
using Business.ResponseOutputs;
using Domain.Entities;
using Domain.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Business.Handlers.Members
{
  public class CreateMemberHandler : IPipelineBehavior<CreateMemberCommand, ApiResponse<Member>>
  {
    private readonly IMemberRepository _memberRepository;

    public CreateMemberHandler(IMemberRepository memberRepository)
    {
      _memberRepository = memberRepository;
    }

    public async Task<ApiResponse<Member>> Handle(CreateMemberCommand request,
        CancellationToken cancellationToken, RequestHandlerDelegate<ApiResponse<Member>> next)
    {
      return new ApiResponse<Member>(await _memberRepository.InsertAsync(new Member(request.Name)));
    }
  }
}
