using DataAccess.Validations;
using Domain.Entities;
using Domain.Validations;
using JetBrains.Annotations;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAccess.Entities
{
    [Serializable]
    public class Member : BaseEntity, IMember
    {
        public Member() { }
        public Member([NotNull] string name)
        {
            Name = name;
        }
        public virtual string Name { get; protected set; }
        private bool isGuildMaster = false;
        public virtual bool IsGuildMaster
        {
            get => isGuildMaster;
            protected set
            {
                if (value) BePromoted();
                else BeDemoted();
            }
        }
        public virtual Guid? GuildId { get; protected set; }
        [JsonIgnore] public virtual Guild Guild { get; protected set; }
        [JsonIgnore] public virtual ICollection<Membership> Memberships { get; protected set; } = new List<Membership>();
        public virtual void ChangeName([NotNull] string newName)
        {
            Name = newName;
        }
        public virtual IMember JoinGuild([NotNull] IInvite invite)
        {
            if (invite is Invite receivedInvite
                && receivedInvite.Guild is Guild invitingGuild
                && invitingGuild != Guild
                && receivedInvite.Status == Domain.Enums.InviteStatuses.Accepted)
            {
                LeaveGuild();
                Guild = invitingGuild;
                GuildId = Guild.Id;
                Memberships.Add(new Membership(Guild, this));
            }
            return this;
        }
        public virtual IMember BePromoted()
        {
            if (!IsGuildMaster && Guild is Guild)
            {
                Guild.DemoteMaster();
                isGuildMaster = true;
            }
            return this;
        }
        public virtual IMember BeDemoted()
        {
            if (IsGuildMaster && Guild is Guild)
            {
                isGuildMaster = false;
                Guild.PromoteSubstituteFor(this);
            }
            return this;
        }
        public virtual IMember LeaveGuild()
        {
            if (Guild is Guild)
            {
                Memberships.OrderBy(ms => ms.Entrance).LastOrDefault()?.RegisterExit();
                Guild.KickMember(this);
                Guild = null;
            }
            return this;
        }
        public override IValidationResult Validate()
        {
            return string.IsNullOrWhiteSpace(Name)
                ? new BadRequestValidationResult(nameof(Member)).AddValidationErrors(nameof(Name), "Can't be null or empty.")
                : ValidationResult;
        }
        public override bool Equals(object obj) =>  obj is Member member && member.Id == Id;
        public override int GetHashCode() => Id.GetHashCode();
    }
}
