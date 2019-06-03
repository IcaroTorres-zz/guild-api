using System.Collections.Generic;
using System.Linq;
using api.Models;

namespace api.Repositories
{
  public interface IGuildRepository : IRepository<Guild>
  {
    Guild CreateGuild(string guildId, string masterId);
    bool AddMember(string guildId, string memberId);
    bool RemoveMember(string member, string guildId);
    bool Transfer(string guildId, string userId);
    IQueryable<Guild> GetNthGuilds(int count = 20);
  }
}