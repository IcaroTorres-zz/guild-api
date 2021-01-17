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
        }
    }
}
