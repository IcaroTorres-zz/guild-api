using Application.Common.Commands;
using Application.Common.Responses;
using Domain.Models;

namespace Application.Members.Commands.LeaveGuild
{
    public class LeaveGuildCommand : UpdateCommand<Member, MemberResponse>
    {
    }
}