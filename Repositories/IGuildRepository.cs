using System.Collections.Generic;
using System.Linq;
using lumen.api.Models;

namespace lumen.api.Repositories
{
  public interface IGuildRepository : IRepository<Guild>
  {
    Guild CreateGuild(string guildName, string masterName);
    bool AddMember(string guildName, string memberName);
    bool RemoveMember(string member, string guildName);
    bool TransferOwnership(string guildname, string userName);
    IQueryable<Guild> GetNthGuilds(int count = 20);
  }
}