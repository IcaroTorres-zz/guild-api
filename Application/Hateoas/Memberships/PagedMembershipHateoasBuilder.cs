using Business.Dtos;
using Business.Usecases.Memberships.ListMemberships;
using Domain.Models;
using HateoasNet.Abstractions;

namespace Application.Hateoas.Memberships
{
    public class PagedMembershipHateoasBuilder : IHateoasSourceBuilder<Pagination<MembershipDto>>
    {
        public void Build(IHateoasSource<Pagination<MembershipDto>> source)
        {
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
        }
    }
}
