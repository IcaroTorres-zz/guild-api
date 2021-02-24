using Application.Common.Abstractions;
using Application.Common.Results;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Invites.Queries.GetInvite
{
    public class GetInviteHandler : IRequestHandler<GetInviteCommand, IApiResult>
    {
        private readonly IInviteRepository _inviteRepository;

        public GetInviteHandler(IInviteRepository inviteRepository)
        {
            _inviteRepository = inviteRepository;
        }

        public async Task<IApiResult> Handle(GetInviteCommand command, CancellationToken cancellationToken)
        {
            var result = new ApiResult();
            var invite = await _inviteRepository.GetByIdAsync(command.Id, true, cancellationToken);
            return result.SetResult(invite);
        }
    }
}