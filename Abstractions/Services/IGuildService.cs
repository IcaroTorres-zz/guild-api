using Abstractions.Entities;
using DTOs;
using Implementations.Entities;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;

namespace Abstractions.Services
{
    public interface IGuildService
    {
        IGuild Get(Guid id);
        IGuild Create(GuildDto payload);
        IGuild Update(GuildDto payload, Guid id);
        IGuild Patch(Guid id, JsonPatchDocument<Guild> payload);
        IGuild AddMember(Guid id, string memberName);
        IGuild RemoveMember(Guid id, string memberName);
        IGuild ChangeGuildMaster(Guid id, string masterName);
        IGuild Delete(Guid id);
        IReadOnlyList<IGuild> List(int count = 20);
    }
}