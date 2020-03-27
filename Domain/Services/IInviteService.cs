using Domain.DTOs;
using Domain.Models;
using System;
using System.Collections.Generic;

namespace Domain.Services
{
    public interface IInviteService
    {
        InviteModel Get(Guid id, bool readOnly = false);
        IReadOnlyList<InviteModel> List(InviteDto payload);
        InviteModel InviteMember(InviteDto payload);
        InviteModel Accept(Guid id);
        InviteModel Decline(Guid id);
        InviteModel Cancel(Guid id);
        InviteModel Delete(Guid id);
    }
}
