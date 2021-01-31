using Business.Commands;
using Business.Dtos;
using Domain.Models;

namespace Business.Usecases.Members.PromoteMember
{
    public class PromoteMemberCommand : UpdateCommand<Member, MemberDto>
    {
    }
}