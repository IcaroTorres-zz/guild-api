using System;
using System.Collections.Generic;
using Domain.Entities;
using Domain.Validations;

namespace DataAccess.Entities.NullEntities
{
    public class NullGuild : Guild
    {
        public NullGuild()
        {
            Id = Guid.Empty;
            Members = new List<Member>();
            Invites = new List<Invite>();
        }
        public override IValidationResult ValidationResult { get => new NotFoundValidationResult($"Unable to find requested {nameof(Guild)}."); set {} }
        public override IValidationResult Validate() => ValidationResult;
        public override void ChangeName(string newName) { }
        public override IInvite Invite(IMember member) => null;
        public override IInvite CancelInvite(IInvite invite) => invite;
        public override IMember AcceptMember(IMember member) => member;
        public override IMember Promote(IMember member) => member;
        public override IMember KickMember(IMember member) => member;
        public override IEnumerable<IMember> UpdateMembers(IEnumerable<IMember> members) => members;
        public override void PromoteMasterSubstitute() { }
        public override void DemoteMaster() { }
    }
}
