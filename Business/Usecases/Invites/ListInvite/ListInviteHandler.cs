using AutoMapper;
using Business.Dtos;
using Business.Responses;
using Domain.Models;
using Domain.Repositories;
using Domain.Responses;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Business.Usecases.Invites.ListInvite
{
    public class ListInviteHandler : IRequestHandler<ListInviteCommand, IApiResult>
    {
        private readonly IInviteRepository _inviteRepository;
        private readonly IMapper _mapper;

        public ListInviteHandler(IInviteRepository inviteRepository, IMapper mapper)
        {
            _inviteRepository = inviteRepository;
            _mapper = mapper;
        }

        public async Task<IApiResult> Handle(ListInviteCommand command, CancellationToken cancellationToken)
        {
            var result = new ApiResult();

            var pagedInvites = await _inviteRepository.PaginateAsync(
                predicate: x =>
                    (command.MemberId == null || x.MemberId == command.MemberId) &&
                    (command.GuildId == null || x.GuildId == command.GuildId),
                top: command.PageSize,
                page: command.Page,
                cancellationToken);

            var pagedResult = _mapper.Map<Pagination<InviteDto>>(pagedInvites);
            pagedResult.SetAppliedCommand(command);
            return result.SetResult(pagedResult);
        }
    }
}