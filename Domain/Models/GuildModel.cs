using DataAccess.Entities;
using Domain.Models.NullEntities;
using Domain.Validations;
using FluentValidation;
using JetBrains.Annotations;
using System.Linq;

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
        public bool IsGuildMember(MemberModel member)
        {
            return Entity.Members.Contains(member.Entity);
        }

        public virtual InviteModel Invite([NotNull] MemberModel member)
        {
            if (member is MemberModel && member.Entity is Member memberToInvite && !Entity.Members.Contains(memberToInvite))
            {
                var inviteModel = new InviteModel(this, member);
                Entity.Invites.Add(inviteModel.Entity);
                return inviteModel;
            }

            return new NullInvite();
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
            return member switch
            {
                MemberModel memberToPromote when Entity.Members.Contains(memberToPromote.Entity) => memberToPromote.BePromoted(),
                _ => member,
            };
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
        public override IApiValidationResult Validate()
        {
            RuleFor(x => x.Name).NotEmpty();
            
            RuleFor(x => x.Members)
                .Must(x => x.Any(m => m.IsGuildMaster))
                .ForEach(memberRule => memberRule
                    .NotEmpty()
                    .Must(x => !x.Disabled)
                    .Must(x => x.GuildId == Entity.Id)
                    .Must(x => x.Guild.Invites.Any(i => i.MemberId == x.Id)))
                .Unless(x => !x.Members?.Any() ?? true);

            RuleForEach(x => x.Invites)
                .NotEmpty()
                .Must(x => !x.Disabled)
                .Must(x => x.GuildId == Entity.Id)
                .Unless(x => x.Invites == null);

            return base.Validate();
        }
    }
}