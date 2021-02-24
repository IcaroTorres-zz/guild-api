using Application.Common.Responses;
using Application.Guilds.Commands.UpdateGuild;
using Application.Guilds.Queries.GetGuild;
using Application.Invites.Queries.ListInvite;
using Application.Members.Queries.GetMember;
using HateoasNet.Abstractions;

namespace Infrastructure.Hateoas.Guilds
{
    public class GuildHateoasBuilder : IHateoasSourceBuilder<GuildResponse>
    {
        public void Build(IHateoasSource<GuildResponse> source)
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
