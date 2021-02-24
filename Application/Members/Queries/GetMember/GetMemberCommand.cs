using Application.Common.Commands;
using Application.Common.Responses;
using Domain.Models;

namespace Application.Members.Queries.GetMember
{
    public class GetMemberCommand : QueryItemCommand<Member, MemberResponse>
    {
    }
}