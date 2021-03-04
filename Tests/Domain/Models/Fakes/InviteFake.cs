using Bogus;
using Domain.Enums;
using Domain.Models;
using System;
using Tests.Domain.Models.TestModels;
using Tests.Helpers;

namespace Tests.Domain.Models.Fakes
{
    public static class InviteFake
    {
        public static Faker<Invite> InvalidWithoutGuild()
        {
            var member = MemberFake.WithoutGuild().Generate();

            return new Faker<Invite>().CustomInstantiator(_ => new TestInvite
            {
                Id = Guid.NewGuid(),
                Member = member,
                MemberId = member.Id
            });
        }

        public static Faker<Invite> InvalidWithoutMember()
        {
            var guild = GuildFake.Valid().Generate();

            return new Faker<Invite>().CustomInstantiator(_ => new TestInvite
            {
                Id = Guid.NewGuid(),
                Guild = guild,
                GuildId = guild.Id
            });
        }

        public static Faker<Invite> ValidWithStatus(InviteStatuses status = InviteStatuses.Pending, Guild guild = null, Member member = null)
        {
            return new Faker<Invite>().CustomInstantiator(_ =>
            {
                member ??= MemberFake.GuildMember().Generate();
                guild ??= GuildFake.Valid().Generate();
                var invite = guild.InviteMember(member, TestModelFactoryHelper.Factory);
                if (status == InviteStatuses.Accepted) invite.BeAccepted(TestModelFactoryHelper.Factory);
                else if (status == InviteStatuses.Denied) invite.BeDenied();
                else if (status == InviteStatuses.Canceled) invite.BeCanceled();
                return invite;
            });
        }

        public static Faker<Invite> ValidToAcceptWithInvitesToCancel(int canceledCount = 5, Guild guild = null, Member member = null)
        {
            return new Faker<Invite>().CustomInstantiator(_ =>
            {
                var invites = ValidWithStatus(InviteStatuses.Pending, guild, member).Generate(Math.Abs(canceledCount) + 1);
                return invites[0];
            });
        }
    }
}
