using AutoMapper;
using Business.Dtos;
using Business.Responses;
using Domain.Repositories;
using Domain.Responses;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Business.Usecases.Invites.DenyInvite
{
    public class DenyInviteHandler : IRequestHandler<DenyInviteCommand, IApiResult>
    {
        private readonly IInviteRepository _inviteRepository;
        private readonly IMapper _mapper;

        public DenyInviteHandler(IInviteRepository inviteRepository, IMapper mapper)
        {
            _inviteRepository = inviteRepository;
            _mapper = mapper;
        }

        public async Task<IApiResult> Handle(DenyInviteCommand command, CancellationToken cancellationToken)
        {
            var result = new ApiResult();

            var invite = await _inviteRepository.GetByIdAsync(command.Id, readOnly: false, cancellationToken);
            _inviteRepository.Update(invite.BeDenied());
            var inviteResult = _mapper.Map<InviteDto>(invite);

            return result.SetResult(inviteResult);
        }
    }
}