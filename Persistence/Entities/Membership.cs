using System;

namespace Persistence.Entities
{
    /// <summary>
    /// Represents a relationship between a <see cref="Member"/> and a <see cref="Guild"/> expressing its beginning and end.
    /// </summary>
    public class Membership : Domain.Models.Membership
  {
        public override DateTime CreatedDate { get; protected set; }
        public override DateTime? ModifiedDate { get; protected set; }
        public override Guid? MemberId { get; protected set; }
        public override Guid? GuildId { get; protected set; }
        public override Domain.Models.Guild Guild { get; protected set; }
        public override Domain.Models.Member Member { get; protected set; }
    }
}