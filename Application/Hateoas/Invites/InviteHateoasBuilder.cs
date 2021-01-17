using Business.Dtos;
using Business.Usecases.Guilds.GetGuild;
using Business.Usecases.Invites.AcceptInvite;
using Business.Usecases.Invites.CancelInvite;
using Business.Usecases.Invites.DenyInvite;
using Business.Usecases.Invites.GetInvite;
using Domain.Enums;
using HateoasNet.Abstractions;

namespace Application.Hateoas.Invites
{
    public class InviteHateoasBuilder : IHateoasSourceBuilder<InviteDto>
    {
        public void Build(IHateoasSource<InviteDto> source)
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
