using Domain.Entities;
using Domain.DTOs;
using DataAccess.Entities;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;

namespace Domain.Services
{
    public interface IMemberService
    {
        IMember Get(Guid id, bool readOnly = false);
        IMember Create(MemberDto payload);
        IMember Update(MemberDto payload, Guid id);
        IMember Patch(Guid id, JsonPatchDocument<Member> payload);
        IMember Promote(Guid id);
        IMember Demote(Guid id);
        IMember LeaveGuild(Guid id);
        IMember Delete(Guid id);
        IReadOnlyList<IMember> List(MemberFilterDto payload);
    }
}