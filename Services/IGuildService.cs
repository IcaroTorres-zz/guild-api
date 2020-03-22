using Domain.DTOs;
using Domain.Models;
using System;
using System.Collections.Generic;

namespace Services
{
    public interface IGuildService
    {
        GuildModel Get(Guid id, bool readOnly = false);
        GuildModel Create(GuildDto payload);
        GuildModel Update(GuildDto payload, Guid id);
        GuildModel Delete(Guid id);
        IReadOnlyList<GuildModel> List(int count = 20);
    }
}