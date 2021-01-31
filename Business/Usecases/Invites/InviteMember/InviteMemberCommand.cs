using Business.Commands;
using Business.Dtos;
using Domain.Models;
using System;

namespace Business.Usecases.Invites.InviteMember
{
    public class InviteMemberCommand : CreationCommand<Invite, InviteDto>
    {
        public Guid MemberId { get; set; }
        public Guid GuildId { get; set; }
    }
}