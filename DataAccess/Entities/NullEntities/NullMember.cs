using System;
using System.Collections.Generic;
using Domain.Entities;
using Domain.Validations;

namespace DataAccess.Entities.NullEntities
{
    public class NullMember : Member
    {
        public NullMember()
        {
            Id = Guid.Empty;
            Memberships = new List<Membership>();
            Guild = new NullGuild();
        }
        public override IValidationResult ValidationResult { get => new NotFoundValidationResult($"Unable to find requested {nameof(Member)}."); set { } }
        public override IValidationResult Validate() => ValidationResult;
        public override bool IsGuildMaster { get => false; protected set { } }
        public override Guid? GuildId { get => Guid.Empty; protected set { } }
        public override void ChangeName(string newName) { }
        public override IMember JoinGuild(IInvite invite) => this;
        public override IMember BePromoted() => this;
        public override IMember BeDemoted() => this;
        public override IMember LeaveGuild() => this;
    }
}
