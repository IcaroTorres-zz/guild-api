using Application.Common.Commands;
using Application.Common.Responses;
using Domain.Models;

namespace Application.Guilds.Queries.GetGuild
{
    public class GetGuildCommand : QueryItemCommand<Guild, GuildResponse>
    {
    }
}