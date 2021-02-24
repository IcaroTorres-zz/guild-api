using Application.Common.Commands;
using Application.Common.Responses;
using Domain.Models;
using System;

namespace Application.Invites.Commands.InviteMember
{
    public class InviteMemberCommand : CreationCommand<Invite, InviteResponse>
    {
        public Guid MemberId { get; set; }
        public Guid GuildId { get; set; }
    }
}