using Application.Common.Commands;
using Application.Common.Responses;
using Domain.Models;
using System;

namespace Application.Guilds.Queries.ListGuild
{
    [Serializable]
    public class ListGuildCommand : QueryListCommand<PagedResponse<Guild>, PagedResponse<GuildResponse>>
    {
    }
}