using JetBrains.Annotations;
using System;

namespace Domain.Entities
{
    public interface IMember : IBaseEntity
    {
        bool IsGuildMaster { get; }
        void ChangeName([NotNull] string newName);
        IMember JoinGuild([NotNull] IInvite invite);
        IMember BePromoted();
        IMember BeDemoted();
        IMember LeaveGuild();
    }
}
