using Application.Common.Responses;
using Application.Guilds.Queries.GetGuild;
using Application.Invites.Commands.AcceptInvite;
using Application.Invites.Commands.CancelInvite;
using Application.Invites.Commands.DenyInvite;
using Application.Invites.Queries.GetInvite;
using Domain.Enums;
using HateoasNet.Abstractions;

namespace Infrastructure.Hateoas.Invites
{
    public class InviteHateoasBuilder : IHateoasSourceBuilder<InviteResponse>
    {
        public void Build(IHateoasSource<InviteResponse> source)
        {
            source.AddLink("get-invite")
                .HasRouteData(x => new GetInviteCommand { Id = x.Id })
                .PresentedAs("details");

            source.AddLink("accept-invite")
                .HasRouteData(x => new AcceptInviteCommand { Id = x.Id })
                .When(x => x.Status == InviteStatuses.Pending && x.Guild != null && x.Member != null)
                .PresentedAs("accept");

            source.AddLink("deny-invite")
                .HasRouteData(x => new DenyInviteCommand { Id = x.Id })
                .When(x => x.Status == InviteStatuses.Pending)
                .PresentedAs("deny");

            source.AddLink("cancel-invite")
                .HasRouteData(x => new CancelInviteCommand { Id = x.Id })
                .When(x => x.Status == InviteStatuses.Pending)
                .PresentedAs("cancelInvite");

            source.AddLink("get-guild")
                .HasRouteData(x => new GetGuildCommand { Id = x.Guild?.Id ?? default })
                .When(x => x.Guild != null)
                .PresentedAs("invitingGuild");

            source.AddLink("get-member")
                .HasRouteData(x => new GetGuildCommand { Id = x.Guild?.Id ?? default })
                .When(x => x.Guild != null)
                .PresentedAs("invitedMember");
        }
    }
}
