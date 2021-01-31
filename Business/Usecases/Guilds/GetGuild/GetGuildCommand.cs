using Business.Commands;
using Business.Dtos;
using Domain.Models;

namespace Business.Usecases.Guilds.GetGuild
{
    public class GetGuildCommand : QueryItemCommand<Guild, GuildDto>
    {
    }
}