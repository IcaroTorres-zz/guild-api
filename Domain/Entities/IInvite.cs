using Domain.Enums;
using System;

namespace Domain.Entities
{
    public interface IInvite : IBaseEntity
    {
        InviteStatuses Status { get; }
        public IInvite BeAccepted();
        public IInvite BeDeclined();
        public IInvite BeCanceled();
    }
}
