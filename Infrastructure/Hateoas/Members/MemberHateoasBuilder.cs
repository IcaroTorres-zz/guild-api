using Application.Common.Responses;
using Application.Guilds.Queries.GetGuild;
using Application.Invites.Queries.ListInvite;
using Application.Members.Commands.ChangeMemberName;
using Application.Members.Commands.DemoteMember;
using Application.Members.Commands.LeaveGuild;
using Application.Members.Commands.PromoteMember;
using Application.Members.Queries.GetMember;
using HateoasNet.Abstractions;

namespace Infrastructure.Hateoas.Members
{
    public class MemberHateoasBuilder : IHateoasSourceBuilder<MemberResponse>
    {
        public void Build(IHateoasSource<MemberResponse> source)
        {
            source.AddLink("get-member")
                .HasRouteData(x => new GetMemberCommand { Id = x.Id })
                .PresentedAs("details");

            source.AddLink("change-member-name")
                .HasRouteData(x => new ChangeMemberNameCommand { Id = x.Id })
                .PresentedAs("changeName");

            source.AddLink("promote-member")
                .HasRouteData(x => new PromoteMemberCommand { Id = x.Id })
                .When(x => x.Guild != null && !x.IsGuildLeader)
                .PresentedAs("promote");

            source.AddLink("demote-member")
                .HasRouteData(x => new DemoteMemberCommand { Id = x.Id })
                .When(x => x.Guild != null && x.IsGuildLeader)
                .PresentedAs("demote");

            source.AddLink("leave-guild")
                .HasRouteData(x => new LeaveGuildCommand { Id = x.Id })
                .When(x => x.Guild != null)
                .PresentedAs("leaveGuild");

            source.AddLink("get-guild")
                .HasRouteData(x => new GetGuildCommand { Id = x.Guild?.Id ?? default })
                .When(x => x.Guild != null)
                .PresentedAs("memberGuild");

            source.AddLink("get-invites")
                .HasRouteData(x => new ListInviteCommand { MemberId = x.Id })
                .PresentedAs("memberInvites");
        }
    }
}
