using Domain.Common;
using Domain.Enums;
using Domain.Models;
using System;
using Tests.Domain.Models.Proxies;

namespace Tests.Domain.Factories
{
    public class TestModelFactory : IModelFactory
    {
        public Guild CreateGuild(string name, Member member)
        {
            var guild = new GuildTestProxy { Id = Guid.NewGuid(), Name = name };
            var invite = guild.InviteMember(member, this);
            invite.BeAccepted(this);
            return guild;
        }

        public Invite CreateInvite(Guild guild, Member member)
        {
            return new InviteTestProxy
            {
                Id = Guid.NewGuid(),
                guild = guild,
                member = member,
                GuildId = guild.Id,
                MemberId = member.Id,
                Status = InviteStatuses.Pending,
            };
        }

        public Member CreateMember(string name)
        {
            return new MemberTestProxy { Id = Guid.NewGuid(), Name = name };
        }

        public Membership CreateMembership(Guild guild, Member member)
        {
            return new MembershipTestProxy
            {
                Id = Guid.NewGuid(),
                GuildName = guild.Name,
                MemberName = member.Name,
                GuildId = guild.Id,
                MemberId = member.Id,
                CreatedDate = DateTime.UtcNow,
            };
        }
    }
}
