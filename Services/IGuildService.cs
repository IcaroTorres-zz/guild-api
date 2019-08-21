using Context;
using DTOs;
using Entities;
using System;
using System.Linq;

namespace Services
{
    public interface IGuildService : IService<ApiContext>
    {
        Guild CreateGuild(GuildDto payload);
        Guild UpdateGuild(GuildDto payload);
        Guild AddMember(Guid id, string memberName);
        Guild RemoveMember(Guid id, string memberName);
        Guild Transfer(Guid id, string masterName);
        IQueryable<Guild> List(int count = 20);
    }
}