using AutoMapper;
using Business.Dtos;
using Business.Responses;
using Domain.Repositories;
using Domain.Responses;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Business.Usecases.Invites.CancelInvite
{
    public class CancelInviteHandler : IRequestHandler<CancelInviteCommand, IApiResult>
    {
        private readonly IInviteRepository _inviteRepository;
        private readonly IMapper _mapper;

        public CancelInviteHandler(IInviteRepository inviteRepository, IMapper mapper)
        {
            _inviteRepository = inviteRepository;
            _mapper = mapper;
        }

        public async Task<IApiResult> Handle(CancelInviteCommand command, CancellationToken cancellationToken)
        {
            var response = new ApiResult();

            var invite = await _inviteRepository.GetByIdAsync(command.Id, readOnly: false, cancellationToken);
            _inviteRepository.Update(invite.BeCanceled());
            var inviteResult = _mapper.Map<InviteDto>(invite);

            return response.SetResult(inviteResult);
        }
    }
}