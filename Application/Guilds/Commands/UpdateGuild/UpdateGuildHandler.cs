using Application.Common.Abstractions;
using Application.Common.Results;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Guilds.Commands.UpdateGuild
{
    public class UpdateGuildHandler : IRequestHandler<UpdateGuildCommand, IApiResult>
    {
        private readonly IGuildRepository _guildRepository;
        private readonly IMemberRepository _memberRepository;

        public UpdateGuildHandler(IGuildRepository guildRepository, IMemberRepository memberRepository)
        {
            _guildRepository = guildRepository;
            _memberRepository = memberRepository;
        }

        public async Task<IApiResult> Handle(UpdateGuildCommand command, CancellationToken cancellationToken)
        {
            var guild = await _guildRepository.GetByIdAsync(command.Id, readOnly: false, cancellationToken);
            var newLeader = guild.Members.Single(x => x.Id.Equals(command.LeaderId));
            var previousLeader = guild.GetLeader();

            guild.ChangeName(command.Name)
                 .Promote(newLeader);

            guild = _guildRepository.Update(guild);
            _memberRepository.Update(newLeader);
            _memberRepository.Update(previousLeader);

            return new SuccessResult(guild);
        }
    }
}