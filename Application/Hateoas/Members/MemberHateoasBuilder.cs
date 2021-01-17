using Business.Dtos;
using Business.Usecases.Guilds.GetGuild;
using Business.Usecases.Invites.ListInvite;
using Business.Usecases.Members.ChangeMemberName;
using Business.Usecases.Members.DemoteMember;
using Business.Usecases.Members.GetMember;
using Business.Usecases.Members.LeaveGuild;
using Business.Usecases.Members.PromoteMember;
using HateoasNet.Abstractions;

namespace Application.Hateoas.Members
{
    public class MemberHateoasBuilder : IHateoasSourceBuilder<MemberDto>
    {
        public void Build(IHateoasSource<MemberDto> source)
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
