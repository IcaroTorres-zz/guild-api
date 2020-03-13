using JetBrains.Annotations;
using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public interface IGuild : IBaseEntity
    {
        void ChangeName([NotNull] string newName);
        IInvite Invite([NotNull] IMember newMember);
        IInvite CancelInvite([NotNull] IInvite invite);
        IMember AcceptMember([NotNull] IMember member);
        IMember Promote([NotNull] IMember newMaster);
        IMember KickMember([NotNull] IMember member);
        IEnumerable<IMember> UpdateMembers(IEnumerable<IMember> members);
    }
}