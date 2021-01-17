using Business.Dtos;
using Business.Usecases.Guilds.ListGuild;
using Domain.Models;
using HateoasNet.Abstractions;

namespace Application.Hateoas.Guilds
{
    public class PagedGuildHateoasBuilder : IHateoasSourceBuilder<Pagination<GuildDto>>
    {
        public void Build(IHateoasSource<Pagination<GuildDto>> source)
        {
            source.AddLink("create-guild").PresentedAs("new");

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
