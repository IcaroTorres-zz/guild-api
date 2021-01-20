using AutoMapper;
using Business.Dtos;
using Business.Responses;
using Domain.Models;
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
        private readonly IMapper _mapper;

        public ListMembershipHandler(IMembershipRepository membershipRepository, IMapper mapper)
        {
            _membershipRepository = membershipRepository;
            _mapper = mapper;
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

            var pagedResult = _mapper.Map<Pagination<MembershipDto>>(pagedMemberships);
            pagedResult.SetAppliedCommand(command);
            return result.SetResult(pagedResult);
        }
    }
}