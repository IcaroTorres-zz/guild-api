using System;
using System.Diagnostics.CodeAnalysis;

namespace Application.Common.Responses
{
    [Serializable, ExcludeFromCodeCoverage]
    public class MembershipResponse
    {
        public Guid Id { get; set; }
        public Guid GuildId { get; set; }
        public string GuildName { get; set; }
        public Guid MemberId { get; set; }
        public string MemberName { get; set; }
        public DateTime Since { get; set; }
        public DateTime? Until { get; set; }
    }
}
