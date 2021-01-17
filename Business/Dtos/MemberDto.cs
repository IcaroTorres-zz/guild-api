using System;

namespace Business.Dtos
{
    [Serializable]
    public class MemberDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsGuildLeader { get; set; }
        public MemberGuildDto Guild { get; set; }
    }

    [Serializable]
    public class MemberGuildDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
