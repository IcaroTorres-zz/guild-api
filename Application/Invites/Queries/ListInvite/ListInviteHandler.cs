using Application.Common.Abstractions;
using Application.Common.Results;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Invites.Queries.ListInvite
{
    public class ListInviteHandler : IRequestHandler<ListInviteCommand, IApiResult>
    {
        private readonly IInviteRepository _inviteRepository;

        public ListInviteHandler(IInviteRepository inviteRepository)
        {
            _inviteRepository = inviteRepository;
        }

        public async Task<IApiResult> Handle(ListInviteCommand command, CancellationToken cancellationToken)
        {
            var pagedInvites = await _inviteRepository.PaginateAsync(
                predicate: x =>
                    (command.MemberId == null || x.MemberId == command.MemberId) &&
                    (command.GuildId == null || x.GuildId == command.GuildId),
                top: command.PageSize,
                page: command.Page,
                cancellationToken);

            pagedInvites.SetAppliedCommand(command);
            return new SuccessResult(pagedInvites);
        }
    }
}