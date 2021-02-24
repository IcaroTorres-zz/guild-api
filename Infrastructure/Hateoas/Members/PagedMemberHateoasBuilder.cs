using Application.Common.Responses;
using Application.Members.Queries.ListMember;
using HateoasNet.Abstractions;

namespace Infrastructure.Hateoas.Members
{
    public class PagedMemberHateoasBuilder : IHateoasSourceBuilder<PagedResponse<MemberResponse>>
    {
        public void Build(IHateoasSource<PagedResponse<MemberResponse>> source)
        {
            source.AddLink("create-member").PresentedAs("new");

            source.AddLink("get-members")
                .HasRouteData(x =>
                {
                    var queryBy = x.GetCommandAs<ListMemberCommand>() ?? new ListMemberCommand
                    {
                        PageSize = x.PageSize
                    };
                    queryBy.Page = 1;
                    return queryBy;
                })
                .When(x => x.Pages > 1 && x.Page > 1)
                .PresentedAs("first");

            source.AddLink("get-members")
                .HasRouteData(x =>
                {
                    var queryBy = x.GetCommandAs<ListMemberCommand>() ?? new ListMemberCommand
                    {
                        PageSize = x.PageSize,
                        Page = x.Page
                    };
                    queryBy.Page++;
                    return queryBy;
                })
                .When(x => x.Page < x.Pages)
                .PresentedAs("next");

            source.AddLink("get-members")
                .HasRouteData(x =>
                {
                    var queryBy = x.GetCommandAs<ListMemberCommand>() ?? new ListMemberCommand
                    {
                        PageSize = x.PageSize,
                        Page = x.Page
                    };
                    queryBy.Page--;
                    return queryBy;
                })
                .When(x => x.Page > 1)
                .PresentedAs("previous");

            source.AddLink("get-members")
                .HasRouteData(x =>
                {
                    var queryBy = x.GetCommandAs<ListMemberCommand>() ?? new ListMemberCommand
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
