using Microsoft.AspNetCore.Mvc;

namespace Business.Usecases.Members.ChangeMemberName
{
    public class ChangeMemberNameCommand : PatchMemberCommand
    {
        [FromBody] public string Name { get; set; }
    }
}