using Domain.Commands;
using System;

namespace Business.Usecases.Invites.InviteMember
{
    public class InviteMemberCommand : CreationCommand
    {
        public Guid MemberId { get; set; }
        public Guid GuildId { get; set; }
    }
}