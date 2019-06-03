using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using api.Context;
using api.DTOs;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Services
{
    public interface IGuildService : IService<ApiContext>
    {
        Guild CreateGuild(GuildDto payload);
        Guild UpdateGuild(GuildDto payload);
        bool AddMember(string guildId, string memberId);
        bool RemoveMember(string member, string guildId);
        bool Transfer(string guildId, string userId);
        IQueryable<Guild> GetNthGuilds(int count = 20);
    }
}