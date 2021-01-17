using Bogus;
using Domain.Enums;
using Domain.Models;
using System;

namespace Tests.Domain.Models.Fakes
{
    public static class InviteFake
    {
        public static Faker<Invite> NullObject()
        {
            return new Faker<Invite>().CustomInstantiator(_ => Invite.Null);
        }

        public static Faker<Invite> InvalidWithoutGuild()
        {
            var member = MemberFake.WithoutGuild().Generate();

            return new Faker<Invite>().CustomInstantiator(_ => new Invite(Guild.Null, member));
        }

        public static Faker<Invite> InvalidWithoutMember()
        {
            var guild = GuildFake.WithGuildLeader().Generate();

            return new Faker<Invite>().CustomInstantiator(_ => new Invite(guild, Member.Null));
        }

        public static Faker<Invite> ValidWithStatus(InviteStatuses status = InviteStatuses.Pending, Guild guild = null, Member member = null)
        {
            return new Faker<Invite>().CustomInstantiator(_ =>
            {
                member ??= MemberFake.GuildMember().Generate();
                guild ??= GuildFake.WithGuildLeader().Generate();
                guild.InviteMember(member);
                var invite = guild.LatestInvite;
                return status switch
                {
                    InviteStatuses.Accepted => invite.BeAccepted(),
                    InviteStatuses.Denied => invite.BeDenied(),
                    InviteStatuses.Canceled => invite.BeCanceled(),
                    _ => invite
                };
            });
        }

        public static Faker<Invite> ValidToAcceptWithInvitesToCancel(int canceledCount = 5, Guild guild = null, Member member = null)
        {
            return new Faker<Invite>().CustomInstantiator(_ =>
            {
                member ??= MemberFake.GuildMember().Generate();
                guild ??= GuildFake.WithGuildLeader().Generate();
                var invites = ValidWithStatus(InviteStatuses.Pending, guild, member).Generate(Math.Abs(canceledCount) + 1);
                return invites[0];
            });
        }
    }
}
