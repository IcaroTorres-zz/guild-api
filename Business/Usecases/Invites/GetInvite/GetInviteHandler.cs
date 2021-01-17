using AutoMapper;
using Business.Dtos;
using Business.Responses;
using Domain.Repositories;
using Domain.Responses;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Business.Usecases.Invites.GetInvite
{
    public class GetInviteHandler : IRequestHandler<GetInviteCommand, IApiResult>
    {
        private readonly IInviteRepository _inviteRepository;
        private readonly IMapper _mapper;

        public GetInviteHandler(IInviteRepository inviteRepository, IMapper mapper)
        {
            _inviteRepository = inviteRepository;
            _mapper = mapper;
        }

        public async Task<IApiResult> Handle(GetInviteCommand command, CancellationToken cancellationToken)
        {
            var response = new ApiResult();
            var inviteModel = await _inviteRepository.GetByIdAsync(command.Id, true, cancellationToken);
            return response.SetResult(_mapper.Map<InviteDto>(inviteModel));
        }
    }
}