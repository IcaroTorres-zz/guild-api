using Business.Commands;
using Business.Dtos;
using Domain.Models;

namespace Business.Usecases.Members.CreateMember
{
    public class CreateMemberCommand : CreationCommand<Member, MemberDto>
    {
        public string Name { get; set; }
    }
}