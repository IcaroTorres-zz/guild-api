using Domain.DTOs;
using Domain.Entities;
using Domain.Models;
using System;

namespace Domain.Services
{
    public interface IInviteService
    {
        Pagination<Invite> List(InviteDto payload);
        InviteModel Get(Guid id, bool readOnly = false);
        InviteModel InviteMember(InviteDto payload);
        InviteModel Accept(Guid id);
        InviteModel Decline(Guid id);
        InviteModel Cancel(Guid id);
        InviteModel Delete(Guid id);
    }
}
