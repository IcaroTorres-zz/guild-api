using Domain.DTOs;
using Domain.Entities;
using Domain.Models;
using Microsoft.AspNetCore.JsonPatch;
using System;

namespace Domain.Services
{
    public interface IMemberService
    {
        Pagination<Member> List(MemberFilterDto payload);
        MemberModel Get(Guid id, bool readOnly = false);
        MemberModel Create(MemberDto payload);
        MemberModel Update(MemberDto payload, Guid id);
        MemberModel Patch(Guid id, JsonPatchDocument<Member> payload);
        MemberModel Promote(Guid id);
        MemberModel Demote(Guid id);
        MemberModel LeaveGuild(Guid id);
        MemberModel Delete(Guid id);
    }
}