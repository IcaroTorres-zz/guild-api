using Business.Dtos;
using Business.Usecases.Invites.ListInvite;
using Domain.Models;
using HateoasNet.Abstractions;

namespace Application.Hateoas.Invites
{
    public class PagedInviteHateoasBuilder : IHateoasSourceBuilder<Pagination<InviteDto>>
    {
        public void Build(IHateoasSource<Pagination<InviteDto>> source)
        {
            source.AddLink("invite-member").PresentedAs("new");

            source.AddLink("get-invites")
                .HasRouteData(x =>
                {
                    var queryBy = x.GetCommandAs<ListInviteCommand>() ?? new ListInviteCommand
                    {
                        PageSize = x.PageSize
                    };
                    queryBy.Page = 1;
                    return queryBy;
                })
                .When(x => x.Pages > 1 && x.Page > 1)
                .PresentedAs("first");

            source.AddLink("get-invites")
                .HasRouteData(x =>
                {
                    var queryBy = x.GetCommandAs<ListInviteCommand>() ?? new ListInviteCommand
                    {
                        PageSize = x.PageSize,
                        Page = x.Page
                    };
                    queryBy.Page++;
                    return queryBy;
                })
                .When(x => x.Page < x.Pages)
                .PresentedAs("next");

            source.AddLink("get-invites")
                .HasRouteData(x =>
                {
                    var queryBy = x.GetCommandAs<ListInviteCommand>() ?? new ListInviteCommand
                    {
                        PageSize = x.PageSize,
                        Page = x.Page
                    };
                    queryBy.Page--;
                    return queryBy;
                })
                .When(x => x.Page > 1)
                .PresentedAs("previous");

            source.AddLink("get-invites")
                .HasRouteData(x =>
                {
                    var queryBy = x.GetCommandAs<ListInviteCommand>() ?? new ListInviteCommand
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
