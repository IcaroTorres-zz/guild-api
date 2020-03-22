using DataAccess.Entities;
using Domain.Validations;
using JetBrains.Annotations;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Domain.Models
{
    public class GuildModel : DomainModel<Guild>
    {
        public GuildModel(Guild entity) : base(entity) { }
        public GuildModel([NotNull] string name, MemberModel member) : base(new Guild())
        {
            Entity.Name = name;
            Invite(member).BeAccepted();
            Promote(member);
        }
        public virtual void ChangeName([NotNull] string newName)
        {
            Entity.Name = newName;
        }
        public bool IsGuildMember(MemberModel member) => Entity.Members.Contains(member.Entity);
        public virtual InviteModel Invite([NotNull] MemberModel member)
        {
            if (member is MemberModel && member.Entity is Member memberToInvite && !Entity.Members.Contains(memberToInvite))
            {
                var newInvite = new Invite { MemberId = memberToInvite.Id, GuildId = Entity.Id };
                Entity.Invites.Add(newInvite);
                return new InviteModel(newInvite);
            }

            return null;
        }
        public virtual MemberModel AcceptMember([NotNull] MemberModel member)
        {
            switch (member)
            {
                case MemberModel memberToInvite when !Entity.Members.Contains(memberToInvite.Entity):
                    Entity.Members.Add(memberToInvite.Entity);
                    if (Entity.Members.Count == 1)
                    {
                        memberToInvite.BePromoted();
                    }
                    Entity.Members.Add(memberToInvite.Entity);
                    return memberToInvite;
                default:
                    return member;
            }
        }
        public virtual MemberModel Promote([NotNull] MemberModel member)
        {
            switch (member)
            {
                case MemberModel memberToPromote when Entity.Members.Contains(memberToPromote.Entity):
                    return memberToPromote.BePromoted();
                default:
                    return member;
            }
        }
        public virtual MemberModel KickMember([NotNull] MemberModel member)
        {
            if (member is MemberModel memberToKick && Entity.Members.Contains(memberToKick.Entity))
            {
                if (memberToKick.Entity.IsGuildMaster)
                {
                    memberToKick.BeDemoted();
                }
                Entity.Members.Remove(memberToKick.Entity);
            }
            return member;
        }
        public override IValidationResult Validate()
        {
            IErrorValidationResult result = null;
            if (string.IsNullOrWhiteSpace(Entity.Name))
            {
                result ??= new BadRequestValidationResult(nameof(Guild));
                result.AddValidationErrors(nameof(Entity.Name), "Can't be null or empty.");
            }

            if (Entity.Members is null || !Entity.Members.Any())
            {
                result ??= new BadRequestValidationResult(nameof(Guild));
                result.AddValidationErrors(nameof(Entity.Members), "Can't be null or empty list.");
            }

            if (Entity.Members != null)
            {
                foreach (var error in Entity.Members.Select(m => new MemberModel(m).ValidationResult)
                                                    .OfType<ErrorValidationResult>()
                                                    .SelectMany(m => m.Errors))
                {
                    result ??= new ConflictValidationResult(nameof(Guild));
                    result.AddValidationErrors(error.Key, error.Value.ToArray());
                }
            }

            if (Entity.Invites != null)
            {
                foreach (var error in Entity.Invites.Select(i => new InviteModel(i).ValidationResult)
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