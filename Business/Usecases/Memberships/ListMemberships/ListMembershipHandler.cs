using Business.Responses;
using Domain.Repositories;
using Domain.Responses;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Business.Usecases.Memberships.ListMemberships
{
    public class ListMembershipHandler : IRequestHandler<ListMembershipCommand, IApiResult>
    {
        private readonly IMembershipRepository _membershipRepository;

        public ListMembershipHandler(IMembershipRepository membershipRepository)
        {
            _membershipRepository = membershipRepository;
        }

        public async Task<IApiResult> Handle(ListMembershipCommand command, CancellationToken cancellationToken)
        {
            var result = new ApiResult();

            var pagedMemberships = await _membershipRepository.PaginateAsync(
            predicate: x =>
                    (command.MemberId == null || x.MemberId == command.MemberId) &&
                    (command.GuildId == null || x.GuildId == command.GuildId),
                top: command.PageSize,
                page: command.Page,
                cancellationToken);

            pagedMemberships.SetAppliedCommand(command);
            return result.SetResult(pagedMemberships);
        }
    }
}