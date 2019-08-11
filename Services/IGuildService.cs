using Guild.Context;
using Guild.DTOs;
using System.Linq;

namespace Guild.Services
{
    public interface IGuildService : IService<ApiContext>
    {
        Entities.Guild CreateGuild(GuildDto payload);
        Entities.Guild UpdateGuild(GuildDto payload);
        bool AddMember(string guildId, string memberId);
        bool RemoveMember(string member, string guildId);
        bool Transfer(string guildId, string userId);
        IQueryable<Entities.Guild> List(int count = 20);
    }
}