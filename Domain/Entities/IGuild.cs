using JetBrains.Annotations;
using System.Collections.Generic;

namespace Domain.Entities
{
    public interface IGuild : IBaseEntity
    {
        void ChangeName([NotNull] string newName);
        bool IsGuildMember(IMember member);
        IInvite Invite([NotNull] IMember newMember);
        IMember AcceptMember([NotNull] IMember member);
        IMember Promote([NotNull] IMember newMaster);
        IMember KickMember([NotNull] IMember member);
    }
}