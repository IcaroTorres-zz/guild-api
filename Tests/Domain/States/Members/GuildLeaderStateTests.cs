using Domain.Models;
using Domain.Nulls;
using Domain.States.Members;
using FluentAssertions;
using Tests.Domain.Models.Fakes;
using Tests.Domain.Models.TestModels;
using Tests.Helpers;
using Xunit;

namespace Tests.Domain.States.Members
{
    [Trait("Domain", "Model-State")]
    public class GuildLeaderStateTests
    {
        [Fact]
        public void Constructor_Should_CreateWith_GivenStatus()
        {
            // arrange
            var leader = (TestMember)MemberFake.GuildLeader().Generate();

            // act
            var sut = new GuildLeaderState(leader, leader.Guild);

            // assert
            sut.Guild.Should().BeOfType<TestGuild>().And.Be(leader.Guild);
            sut.IsGuildLeader.Should().BeTrue().And.Be(leader.IsGuildLeader);
            sut.Guild.Members.Should().Contain(leader);
        }

        [Fact]
        public void Join_Should_Change_Guild_And_Memberships()
        {
            // arrange
            var member = (TestMember)MemberFake.GuildMember().Generate();
            var guild = (TestGuild)GuildFake.Complete().Generate();
            var monitor = member.Monitor();
            var sut = member.State;
            var factory = TestModelFactoryHelper.Factory;

            // act
            sut.Join(guild, factory);

            // assert
            sut.Guild.Should().NotBeNull().And.BeOfType<TestGuild>().And.NotBe(member.Guild);
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
            var leader = (TestMember)MemberFake.GuildLeader().Generate();
            var monitor = leader.Monitor();
            var sut = leader.State;

            // act
            sut.BePromoted();

            // assert
            sut.Guild.Should().NotBeNull().And.BeOfType<TestGuild>().And.Be(leader.Guild);
            sut.Guild.Members.Should().Contain(leader);
            sut.IsGuildLeader.Should().BeTrue().And.Be(leader.IsGuildLeader);
            leader.GuildId.Should().NotBeNull();

            monitor.AssertPropertyNotChanged(
                nameof(Member.Id),
                nameof(Member.Name),
                nameof(Member.Guild),
                nameof(Member.GuildId),
                nameof(Member.IsGuildLeader),
                nameof(Member.Memberships));
        }

        [Fact]
        public void BeDemoted_Should_Change_Nothing()
        {
            // arrange
            var leader = (TestMember)MemberFake.GuildLeader().Generate();
            var monitor = leader.Monitor();
            var sut = leader.State;

            // act
            sut.BeDemoted();

            // assert
            sut.Guild.Should().NotBeNull().And.BeOfType<TestGuild>().And.Be(leader.Guild);
            sut.IsGuildLeader.Should().BeTrue().And.Be(!leader.IsGuildLeader);
            sut.Guild.Members.Should().Contain(leader);

            monitor.AssertPropertyChanged(nameof(Member.IsGuildLeader));
            monitor.AssertPropertyNotChanged(
                nameof(Member.Id),
                nameof(Member.Name),
                nameof(Member.Guild),
                nameof(Member.GuildId),
                nameof(Member.Memberships));
        }

        [Fact]
        public void Leave_Should_Change_Guild()
        {
            // arrange
            var leader = (TestMember)MemberFake.GuildLeader().Generate();
            var monitor = leader.Monitor();
            var membership = (TestMembership)leader.GetActiveMembership();
            var membershipMonitor = membership.Monitor();
            var sut = leader.State;

            // act
            sut.Leave();

            // assert
            sut.Guild.Should().BeOfType<TestGuild>().And.NotBe(leader.Guild);
            sut.Guild.Members.Should().Contain(leader);
            sut.IsGuildLeader.Should().BeTrue().And.Be(!leader.IsGuildLeader);
            leader.Guild.Should().BeOfType<NullGuild>();
            leader.GuildId.Should().BeNull();
            leader.Memberships.Should().Contain(membership);

            monitor.AssertPropertyChanged(
                nameof(Member.Guild),
                nameof(Member.GuildId),
                nameof(Member.IsGuildLeader));

            monitor.AssertPropertyNotChanged(
                nameof(Member.Id),
                nameof(Member.Name));
            monitor.AssertCollectionNotChanged(leader.Memberships);

            membershipMonitor.AssertPropertyChanged(nameof(Membership.ModifiedDate));
        }
    }
}