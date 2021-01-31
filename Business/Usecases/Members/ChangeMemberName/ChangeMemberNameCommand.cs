using Business.Commands;
using Business.Dtos;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace Business.Usecases.Members.ChangeMemberName
{
    public class ChangeMemberNameCommand : UpdateCommand<Member, MemberDto>
    {
        [FromBody] public string Name { get; set; }
    }
}