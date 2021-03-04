using Domain.Common;
using Domain.Enums;
using Domain.Models;
using System;

namespace Domain.Factories
{
    public class ModelFactory : IModelFactory
    {
        public Guild CreateGuild(string name, Member member)
        {
            var guild = new Guild { Id = Guid.NewGuid(), Name = name };
            var invite = guild.InviteMember(member, this);
            invite.BeAccepted(this);
            return guild;
        }

        public Invite CreateInvite(Guild guild, Member member)
        {
            return new Invite
            {
                Id = Guid.NewGuid(),
                Guild = guild,
                GuildId = guild.Id,
                Member = member,
                MemberId = member.Id,
                Status = InviteStatuses.Pending,
            };
        }

        public Member CreateMember(string name)
        {
            return new Member { Id = Guid.NewGuid(), Name = name };
        }

        public Membership CreateMembership(Guild guild, Member member)
        {
            return new Membership
            {
                Id = Guid.NewGuid(),
                Guild = guild,
                GuildId = guild.Id,
                Member = member,
                MemberId = member.Id,
                CreatedDate = DateTime.UtcNow,
            };
        }
    }
}
