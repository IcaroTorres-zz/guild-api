using Business.Commands;
using Business.Dtos;
using Domain.Models;

namespace Business.Usecases.Members.DemoteMember
{
    public class DemoteMemberCommand : UpdateCommand<Member, MemberDto>
    {
    }
}