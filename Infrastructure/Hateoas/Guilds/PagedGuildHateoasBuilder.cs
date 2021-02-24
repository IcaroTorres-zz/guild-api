using Application.Common.Responses;
using Application.Guilds.Queries.ListGuild;
using HateoasNet.Abstractions;

namespace Infrastructure.Hateoas.Guilds
{
    public class PagedGuildHateoasBuilder : IHateoasSourceBuilder<PagedResponse<GuildResponse>>
    {
        public void Build(IHateoasSource<PagedResponse<GuildResponse>> source)
        {
            source.AddLink("create-guild").PresentedAs("new");

            source.AddLink("get-guilds")
                .HasRouteData(x =>
                {
                    var queryBy = x.GetCommandAs<ListGuildCommand>() ?? new ListGuildCommand
                    {
                        PageSize = x.PageSize
                    };
                    queryBy.Page = 1;
                    return queryBy;
                })
                .When(x => x.Pages > 1 && x.Page > 1)
                .PresentedAs("first");

            source.AddLink("get-guilds")
                .HasRouteData(x =>
                {
                    var queryBy = x.GetCommandAs<ListGuildCommand>() ?? new ListGuildCommand
                    {
                        PageSize = x.PageSize,
                        Page = x.Page
                    };
                    queryBy.Page--;
                    return queryBy;
                })
                .When(x => x.Page > 1)
                .PresentedAs("previous");

            source.AddLink("get-guilds")
                .HasRouteData(x =>
                {
                    var queryBy = x.GetCommandAs<ListGuildCommand>() ?? new ListGuildCommand
                    {
                        PageSize = x.PageSize,
                        Page = x.Page
                    };
                    queryBy.Page++;
                    return queryBy;
                })
                .When(x => x.Page < x.Pages)
                .PresentedAs("next");

            source.AddLink("get-guilds")
                .HasRouteData(x =>
                {
                    var queryBy = x.GetCommandAs<ListGuildCommand>() ?? new ListGuildCommand
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
