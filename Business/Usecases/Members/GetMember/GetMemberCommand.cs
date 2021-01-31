using Business.Commands;
using Business.Dtos;
using Domain.Models;

namespace Business.Usecases.Members.GetMember
{
    public class GetMemberCommand : QueryItemCommand<Member, MemberDto>
    {
    }
}