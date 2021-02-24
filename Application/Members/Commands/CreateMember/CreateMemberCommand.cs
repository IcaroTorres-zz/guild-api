using Application.Common.Commands;
using Application.Common.Responses;
using Domain.Models;

namespace Application.Members.Commands.CreateMember
{
    public class CreateMemberCommand : CreationCommand<Member, MemberResponse>
    {
        public string Name { get; set; }
    }
}