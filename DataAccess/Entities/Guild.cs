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
    public class Guild : BaseEntity, IGuild
    {
        // EF core suitable parametersless constructor hidden to be called elsewhere
        protected Guild() { }
        public Guild([NotNull] string name, Member member)
        {
            Name = name;
            Invite(member).BeAccepted();
            Promote(member);
        }
        public string Name { get; protected set; }
        [JsonIgnore] public virtual ICollection<Member> Members { get; protected set; } = new List<Member>();
        [JsonIgnore] public virtual ICollection<Invite> Invites { get; protected set; } = new List<Invite>();
        public virtual void ChangeName([NotNull] string newName)
        {
            Name = newName;
        }
        public bool IsGuildMember(IMember member) => Members.Contains(member);
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
                    memberToKick.BeDemoted();
                }
                Members.Remove(memberToKick);
            }
            return member;
        }
        public virtual void PromoteSubstituteFor(IMember previousMaster)
        {
            Members
                .OrderByDescending(m => m.Memberships
                    .SingleOrDefault(ms => ms.Exit == null)
                    ?.GetDuration())
                .FirstOrDefault(m => m.Id != previousMaster.Id && !m.IsGuildMaster)
                ?.BePromoted();
        }
        public override IValidationResult Validate()
        {
            IErrorValidationResult result = null;
            if (string.IsNullOrWhiteSpace(Name))
            {
                result ??= new BadRequestValidationResult(nameof(Guild));
                result.AddValidationErrors(nameof(Name), "Can't be null or empty.");
            }

            if (Members is null || !Members.Any())
            {
                result ??= new BadRequestValidationResult(nameof(Guild));
                result.AddValidationErrors(nameof(Members), "Can't be null or empty list.");
            }

            if (Members != null)
            {
                foreach (var error in Members.Select(m => m.ValidationResult)
                                             .OfType<ErrorValidationResult>()
                                             .SelectMany(m => m.Errors))
                {
                    result ??= new ConflictValidationResult(nameof(Guild));
                    result.AddValidationErrors(error.Key, error.Value.ToArray());
                }
            }

            if (Invites != null)
            {
                foreach (var error in Invites.Select(i => i.ValidationResult)
                                             .OfType<ErrorValidationResult>()
                                             .SelectMany(i => i.Errors))
                {
                    result ??= new ConflictValidationResult(nameof(Guild));
                    result.AddValidationErrors(error.Key, error.Value.ToArray());
                }
            }

            ValidationResult = result ?? ValidationResult;

            return ValidationResult;
        }
    }
}