using Domain.Entities;
using Domain.Validations;
using JetBrains.Annotations;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace DataAccess.Entities
{
    [Serializable]
    public class Guild : BaseEntity, IGuild
    {
        // EF core suitable parametersless constructor hidden to be called elsewhere
        protected Guild()
        {
            ValidationResult = new OkValidationResult(this);
        }
        public Guild([NotNull] string name, Member member) : base()
        {
            Name = name;
            Members.Add(member);
            Promote(member);
            ValidationResult = new CreatedValidationResult(this);
        }
        public string Name { get; protected set; }
        [JsonIgnore] public virtual ICollection<Member> Members { get; protected set; } = new List<Member>();
        [JsonIgnore] public virtual ICollection<Invite> Invites { get; protected set; } = new List<Invite>();
        public virtual void ChangeName([NotNull] string newName)
        {
            Name = newName;
        }
        public virtual IInvite Invite([NotNull] IMember member)
        {
            if (member is Member memberToInvite && !Members.Contains(memberToInvite))
            {
                var invite = new Invite(this, memberToInvite);
                Invites.Add(invite);
                return invite;
            }

            return null;
        }
        public virtual IInvite CancelInvite([NotNull] IInvite invite)
        {
            if (invite is Invite inviteToCancel && Invites.Contains(inviteToCancel))
            {
                inviteToCancel.BeCanceled();
            }
            return invite;
        }
        public virtual IMember AcceptMember([NotNull] IMember member)
        {
            if (member is Member memberToInvite && !Members.Contains(memberToInvite))
            {
                Members.Add(memberToInvite);
                if (Members.Count == 1)
                {
                    memberToInvite.BePromoted();
                }
            }
            return member;
        }
        public virtual IMember Promote([NotNull] IMember member)
        {
            if (member is Member memberToPromote && Members.Contains(memberToPromote))
            {
                DemoteMaster();
                memberToPromote.BePromoted();
            }
            return member;
        }
        public virtual IMember KickMember([NotNull] IMember member)
        {
            if (member is Member memberToKick && Members.Contains(memberToKick))
            {
                if (memberToKick.IsGuildMaster)
                {
                    DemoteMaster();
                    PromoteMasterSubstitute();
                }
                Members.Remove(memberToKick);
            }
            return member;
        }
        public virtual IEnumerable<IMember> UpdateMembers(IEnumerable<IMember> members)
        {
            foreach (var memberToInvite in members)
            {
                Invite(memberToInvite).BeAccepted();
            }
            foreach (var memberToKick in Members.Except(members))
            {
                KickMember(memberToKick);
            }
            return Members;
        }
        public virtual void PromoteMasterSubstitute()
        {
            Members
                .OrderByDescending(m => m.Memberships
                    .SingleOrDefault(ms => ms.Exit == null)
                    .GetDuration())
                .FirstOrDefault(m => !m.IsGuildMaster)
                ?.BePromoted();
        }
        public virtual void DemoteMaster() => Members.SingleOrDefault(m => m.IsGuildMaster)?.BeDemoted();
        public override IValidationResult Validate()
        {
            IValidationResult result = null;
            if (string.IsNullOrWhiteSpace(Name) && Members.Any())
            {
                result = new BadRequestValidationResult($"{Name} Can't be null or empty.");
            }

            if (!Members.Any())
            {
                var errorMessage = $"{nameof(Members)} Can't be null or empty list.";
                result = (result == null)
                    ? new BadRequestValidationResult(errorMessage)
                    : result.AddValidationError(HttpStatusCode.BadRequest, errorMessage);
            }

            return result ?? ValidationResult;
        }
    }
}