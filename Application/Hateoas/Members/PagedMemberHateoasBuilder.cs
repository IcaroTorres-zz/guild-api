using Business.Dtos;
using Business.Usecases.Members.ListMember;
using Domain.Models;
using HateoasNet.Abstractions;

namespace Application.Hateoas.Members
{
    public class PagedMemberHateoasBuilder : IHateoasSourceBuilder<Pagination<MemberDto>>
    {
        public void Build(IHateoasSource<Pagination<MemberDto>> source)
        {
            source.AddLink("create-member").PresentedAs("new");

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
        }
    }
}
