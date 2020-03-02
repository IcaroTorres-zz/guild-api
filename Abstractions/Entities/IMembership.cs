using System;

namespace Abstractions.Entities
{
    public interface IMembership
    {
        Guid Id { get; }
        IMembership RegisterExit();
        TimeSpan GetDuration();
    }
}
