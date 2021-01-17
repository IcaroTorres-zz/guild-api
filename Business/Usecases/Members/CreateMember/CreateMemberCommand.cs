using Domain.Commands;

namespace Business.Usecases.Members.CreateMember
{
    public class CreateMemberCommand : CreationCommand
    {
        public string Name { get; set; }
    }
}