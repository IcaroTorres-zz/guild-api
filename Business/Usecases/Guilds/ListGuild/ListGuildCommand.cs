using Business.Commands;
using Business.Dtos;
using Domain.Models;
using System;

namespace Business.Usecases.Guilds.ListGuild
{
    [Serializable]
    public class ListGuildCommand : QueryListCommand<Pagination<Guild>, Pagination<GuildDto>>
    {
    }
}