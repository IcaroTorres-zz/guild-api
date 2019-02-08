using System.Collections.Generic;
using lumen.api.Models;

namespace lumen.api.Repositories
{
  public interface IGuildRepository : IRepository<Guild>
  {
    bool CreateGuild(string guildName, string masterName);
    bool AddMember(string guildName, string memberName);
    bool RemoveMember(string member, string guildName);
    bool TransferOwnership(string guildname, string userName);
    IEnumerable<string> GetNthGuilds(int count = 20);
    Dictionary<string, dynamic> GuildInfo(string guildName);
    new Guild Get (string name);
  }
}