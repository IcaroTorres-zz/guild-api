using DTOs;
using Entities;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;

namespace Services
{
    public interface IGuildService
    {
        Guild Get(Guid id);
        Guild CreateOrUpdate(GuildDto payload, Guid id = new Guid());
        Guild Update(Guid id, JsonPatchDocument<Guild> payload);
        Guild AddMember(Guid id, string memberName);
        Guild RemoveMember(Guid id, string memberName);
        Guild ChangeGuildMaster(Guid id, string masterName);
        Guild Delete(Guid id);
        IReadOnlyList<Guild> List(int count = 20);
    }
}