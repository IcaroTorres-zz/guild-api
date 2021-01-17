using Business.Dtos;
using Business.Usecases.Guilds.GetGuild;
using Business.Usecases.Guilds.UpdateGuild;
using Business.Usecases.Invites.ListInvite;
using Business.Usecases.Members.GetMember;
using HateoasNet.Abstractions;

namespace Application.Hateoas.Guilds
{
    public class GuildHateoasBuilder : IHateoasSourceBuilder<GuildDto>
    {
        public void Build(IHateoasSource<GuildDto> source)
        {
            source.AddLink("get-guild")
                .HasRouteData(x => new GetGuildCommand { Id = x.Id })
                .PresentedAs("details");

            source.AddLink("update-guild")
                .HasRouteData(x => new UpdateGuildCommand { Id = x.Id })
                .PresentedAs("update");

            source.AddLink("get-member")
                .HasRouteData(x => new GetMemberCommand { Id = x.Leader?.Id ?? default })
                .When(x => x.Leader != null)
                .PresentedAs("guildLeader");

            source.AddLink("get-invites")
                .HasRouteData(x => new ListInviteCommand { GuildId = x.Id })
                .PresentedAs("guildInvites");
        }
    }
}
