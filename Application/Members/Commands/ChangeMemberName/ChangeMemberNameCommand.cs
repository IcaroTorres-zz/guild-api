using Application.Common.Commands;
using Application.Common.Responses;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace Application.Members.Commands.ChangeMemberName
{
    public class ChangeMemberNameCommand : UpdateCommand<Member, MemberResponse>
    {
        [FromBody] public string Name { get; set; }
    }
}