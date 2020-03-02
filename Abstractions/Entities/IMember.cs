using JetBrains.Annotations;
using System;

namespace Abstractions.Entities
{
    public interface IMember
    {
        public Guid Id { get; }
        public bool IsGuildMaster { get; }

        void ChangeName([NotNull] string newName);
        void BePromoted();
        void BeDemoted();
        void AcceptInvitation([NotNull] IGuild invitingGuild);
        void QuitGuild();
    }
}
