using System;
using System.Diagnostics.CodeAnalysis;

namespace Application.Common.Responses
{
    [Serializable, ExcludeFromCodeCoverage]
    public class MemberResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsGuildLeader { get; set; }
        public MemberGuildResponse Guild { get; set; }
    }

    [Serializable, ExcludeFromCodeCoverage]
    public class MemberGuildResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
