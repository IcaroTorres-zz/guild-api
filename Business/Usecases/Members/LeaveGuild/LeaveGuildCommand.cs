using Business.Commands;
using Business.Dtos;
using Domain.Models;

namespace Business.Usecases.Members.LeaveGuild
{
    public class LeaveGuildCommand : UpdateCommand<Member, MemberDto>
    {
    }
}