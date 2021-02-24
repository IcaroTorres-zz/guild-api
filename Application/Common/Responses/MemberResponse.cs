using System;

namespace Application.Common.Responses
{
    [Serializable]
    public class MemberResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsGuildLeader { get; set; }
        public MemberGuildResponse Guild { get; set; }
    }

    [Serializable]
    public class MemberGuildResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
