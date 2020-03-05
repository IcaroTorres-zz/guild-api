using System;

namespace Domain.Entities
{
    public interface IMembership
    {
        Guid Id { get; }
        IMembership RegisterExit();
        TimeSpan GetDuration();
    }
}
