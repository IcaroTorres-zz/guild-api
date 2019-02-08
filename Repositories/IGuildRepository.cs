using System.Collections.Generic;
using lumen.api.Models;

namespace lumen.api.Repositories
{
  public interface IGuildRepository : IRepository<Guild>
  {
    bool CreateGuild(string guildName, User master);
    IEnumerable<string> GetNthGuilds(int count = 20);
    bool AddMember(string guildName, string userName);
    bool RemoveMember(string userName, string guildName);
    Dictionary<string, dynamic> GuildInfo(string guildName);
  }
}