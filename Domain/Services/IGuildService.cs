using Domain.Entities;
using Domain.DTOs;
using System;
using System.Collections.Generic;

namespace Domain.Services
{
    public interface IGuildService
    {
        IGuild Get(Guid id, bool readOnly = false);
        IGuild Create(GuildDto payload);
        IGuild Update(GuildDto payload, Guid id);
        IGuild Delete(Guid id);
        IReadOnlyList<IGuild> List(int count = 20);
    }
}