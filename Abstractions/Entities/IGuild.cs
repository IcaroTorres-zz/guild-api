using JetBrains.Annotations;
using System;
using System.Collections.Generic;

namespace Abstractions.Entities
{
    public interface IGuild
    {
        Guid Id { get; }

        void ChangeName([NotNull] string newName);
        void Invite([NotNull] IMember newMember);
        void Promote([NotNull] IMember newMaster);
        void Demote([NotNull] IMember previousMaster);
        void KickMember([NotNull] IMember member);
        void UpdateMembers(IEnumerable<IMember> members);
    }
}