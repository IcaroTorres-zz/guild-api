using Domain.DTOs;
using Domain.Entities;
using Domain.Models;
using System;

namespace Domain.Services
{
    public interface IGuildService
    {
        Pagination<Guild> List(int count = 20);
        GuildModel Get(Guid id, bool readOnly = false);
        GuildModel Create(GuildDto payload);
        GuildModel Update(GuildDto payload, Guid id);
        GuildModel Delete(Guid id);
    }
}