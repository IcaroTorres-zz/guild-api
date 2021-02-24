using Application.Common.Commands;
using Application.Common.Responses;
using Domain.Models;

namespace Application.Members.Commands.DemoteMember
{
    public class DemoteMemberCommand : UpdateCommand<Member, MemberResponse>
    {
    }
}