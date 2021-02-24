using Application.Common.Responses;
using Application.Memberships.Queries.ListMemberships;
using HateoasNet.Abstractions;

namespace Infrastructure.Hateoas.Memberships
{
    public class PagedMembershipHateoasBuilder : IHateoasSourceBuilder<PagedResponse<MembershipResponse>>
    {
        public void Build(IHateoasSource<PagedResponse<MembershipResponse>> source)
        {
            source.AddLink("get-memberships")
                .HasRouteData(x =>
                {
                    var queryBy = x.GetCommandAs<ListMembershipCommand>() ?? new ListMembershipCommand
                    {
                        PageSize = x.PageSize
                    };
                    queryBy.Page = 1;
                    return queryBy;
                })
                .When(x => x.Pages > 1 && x.Page > 1)
                .PresentedAs("first");

            source.AddLink("get-memberships")
                .HasRouteData(x =>
                {
                    var queryBy = x.GetCommandAs<ListMembershipCommand>() ?? new ListMembershipCommand
                    {
                        PageSize = x.PageSize,
                        Page = x.Page
                    };
                    queryBy.Page++;
                    return queryBy;
                })
                .When(x => x.Page < x.Pages)
                .PresentedAs("next");

            source.AddLink("get-memberships")
                .HasRouteData(x =>
                {
                    var queryBy = x.GetCommandAs<ListMembershipCommand>() ?? new ListMembershipCommand
                    {
                        PageSize = x.PageSize,
                        Page = x.Page
                    };
                    queryBy.Page--;
                    return queryBy;
                })
                .When(x => x.Page > 1)
                .PresentedAs("previous");

            source.AddLink("get-memberships")
                .HasRouteData(x =>
                {
                    var queryBy = x.GetCommandAs<ListMembershipCommand>() ?? new ListMembershipCommand
                    {
                        PageSize = x.PageSize
                    };
                    queryBy.Page = x.Pages;
                    return queryBy;
                })
                .When(x => x.Page < x.Pages && x.Pages > 1)
                .PresentedAs("last");
        }
    }
}
