using Business.Commands.Guilds;
using Business.ResponseOutputs;
using Domain.Entities;
using Domain.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Business.Handlers.Guilds
{
  public class CreateGuildHandler : IPipelineBehavior<CreateGuildCommand, ApiResponse<Guild>>
  {
    private readonly IGuildRepository _guildRepository;
    private readonly IMemberRepository _memberRepository;

    public CreateGuildHandler(IGuildRepository guildRepository, IMemberRepository memberRepository)
    {
      _guildRepository = guildRepository;
      _memberRepository = memberRepository;
    }

    public async Task<ApiResponse<Guild>> Handle(CreateGuildCommand request,
        CancellationToken cancellationToken, RequestHandlerDelegate<ApiResponse<Guild>> next)
    {
      var master = await _memberRepository.GetForGuildOperationsAsync(request.MasterId);

      return new ApiResponse<Guild>(await _guildRepository.InsertAsync(new Guild(request.Name, master)));
    }
  }
}
