using Domain.Models;
using Domain.Nulls;
using Domain.States.Members;
using FluentAssertions;
using Tests.Domain.Models.Fakes;
using Tests.Helpers;
using Xunit;

namespace Tests.Domain.States.Members
{
    [Trait("Domain", "Model-State")]
    public class GuildMemberStateTests
    {
        [Fact]
        public void Constructor_Should_CreateWith_GivenStatus()
        {
            // arrange
            var member = MemberFake.GuildMember().Generate();

            // act
            var sut = new GuildMemberState(member, member.Guild);

            // assert
            sut.Guild.Should().BeOfType<Guild>().And.Be(member.Guild);
            sut.IsGuildLeader.Should().BeFalse().And.Be(member.IsGuildLeader);
            sut.Guild.Members.Should().Contain(member);
        }

        [Fact]
        public void Join_Should_Change_Guild_And_Memberships()
        {
            // arrange
            var member = MemberFake.GuildMember().Generate();
            var guild = GuildFake.Valid().Generate();
            var monitor = member.Monitor();
            var sut = member.State;

            // act
            sut.Join(guild);

            // assert
            sut.Guild.Should().NotBeNull().And.BeOfType<Guild>().And.NotBe(member.Guild);
            sut.IsGuildLeader.Should().BeFalse().And.Be(member.IsGuildLeader);
            sut.Guild.Members.Should().Contain(member);
            guild.Members.Should().Contain(member);
            member.GuildId.Should().NotBeNull();

            monitor.AssertCollectionChanged(member.Memberships);
            monitor.AssertPropertyChanged(
                nameof(Member.Guild),
                nameof(Member.GuildId));

            monitor.AssertPropertyNotChanged(
                nameof(Member.Id),
                nameof(Member.Name),
                nameof(Member.IsGuildLeader));
        }

        [Fact]
        public void BePromoted_Should_Change_IsGuildLeader()
        {
            // arrange
            var member = MemberFake.GuildMember().Generate();
            var sut = new GuildMemberState(member, member.Guild);
            var monitor = member.Monitor();

            // act
            sut.BePromoted();

            // assert
            sut.Guild.Should().NotBeNull().And.BeOfType<Guild>().And.Be(member.Guild);
            sut.Guild.Members.Should().Contain(member);
            sut.IsGuildLeader.Should().BeFalse().And.Be(!member.IsGuildLeader);
            member.GuildId.Should().NotBeNull();

            monitor.AssertPropertyChanged(nameof(Member.IsGuildLeader));
            monitor.AssertPropertyNotChanged(
                nameof(Member.Id),
                nameof(Member.Name),
                nameof(Member.Guild),
                nameof(Member.GuildId));

            monitor.AssertCollectionNotChanged(member.Memberships);
        }

        [Fact]
        public void BeDemoted_Should_Change_Nothing()
        {
            // arrange
            var member = MemberFake.GuildMember().Generate();
            var sut = new GuildMemberState(member, member.Guild);
            var monitor = member.Monitor();

            // act
            sut.BeDemoted();

            // assert
            sut.Guild.Should().NotBeNull().And.BeOfType<Guild>().And.Be(member.Guild);
            sut.IsGuildLeader.Should().BeFalse().And.Be(member.IsGuildLeader);
            sut.Guild.Members.Should().Contain(member);
            member.GuildId.Should().NotBeNull();

            monitor.AssertPropertyNotChanged(
                nameof(Member.Id),
                nameof(Member.Name),
                nameof(Member.Guild),
                nameof(Member.GuildId),
                nameof(Member.IsGuildLeader));
            monitor.AssertCollectionNotChanged(member.Memberships);
        }

        [Fact]
        public void Leave_Should_Change_Guild()
        {
            // arrange
            var member = MemberFake.GuildMember().Generate();
            var monitor = member.Monitor();
            var membership = member.GetActiveMembership();
            var membershipMonitor = membership.Monitor();
            var sut = member.State;

            // act
            sut.Leave();

            // assert
            sut.Guild.Should().BeOfType<Guild>().And.NotBe(member.Guild);
            sut.Guild.Members.Should().Contain(member);
            sut.IsGuildLeader.Should().BeFalse().And.Be(member.IsGuildLeader);
            member.Guild.Should().BeOfType<NullGuild>();
            member.GuildId.Should().BeNull();
            member.Memberships.Should().Contain(membership);

            monitor.AssertPropertyChanged(
                nameof(Member.Guild),
                nameof(Member.GuildId));

            monitor.AssertPropertyNotChanged(
                nameof(Member.Id),
                nameof(Member.Name),
                nameof(Member.IsGuildLeader));
            monitor.AssertCollectionNotChanged(member.Memberships);

            membershipMonitor.AssertPropertyChanged(nameof(Membership.ModifiedDate));
        }
    }
}