using Domain.Models;
using Domain.Nulls;
using Domain.States.Members;
using FluentAssertions;
using Tests.Domain.Models.Fakes;
using Tests.Domain.Models.Proxies;
using Tests.Helpers;
using Xunit;

namespace Tests.Domain.States.Members
{
    [Trait("Domain", "Model-State")]
    public class NoGuildMemberStateTests
    {
        [Fact]
        public void Constructor_Should_CreateWith_GivenStatus()
        {
            // arrange
            var member = (MemberTestProxy)MemberFake.WithoutGuild().Generate();

            // act
            var sut = new NoGuildMemberState(member);

            // assert
            sut.Guild.Should().BeOfType<NullGuild>().And.Be(member.GetGuild());
            sut.IsGuildLeader.Should().BeFalse().And.Be(member.IsGuildLeader);
            sut.Guild.Members.Should().NotContain(member);
        }

        [Fact]
        public void Join_Should_Change_Guild_And_Memberships()
        {
            // arrange
            var member = (MemberTestProxy)MemberFake.WithoutGuild().Generate();
            var monitor = member.Monitor();
            var guild = (GuildTestProxy)GuildFake.Complete().Generate();
            var sut = member.GetState();
            var factory = TestModelFactoryHelper.Factory;

            // act
            sut.Join(guild, factory);

            // assert
            sut.Guild.Should().NotBeNull().And.BeOfType<NullGuild>().And.NotBe(member.GetGuild());
            sut.IsGuildLeader.Should().BeFalse().And.Be(member.IsGuildLeader);
            sut.Guild.Members.Should().NotContain(member);
            member.GuildId.Should().NotBeNull();
            guild.Members.Should().Contain(member);

            monitor.AssertCollectionChanged(member.Memberships);
            monitor.AssertPropertyChanged(
                nameof(Guild),
                nameof(Member.GuildId));

            monitor.AssertPropertyNotChanged(
                nameof(Member.Id),
                nameof(Member.Name),
                nameof(Member.IsGuildLeader));
        }

        [Fact]
        public void BePromoted_Should_Change_Nothing()
        {
            // arrange
            var member = (MemberTestProxy)MemberFake.WithoutGuild().Generate();
            var monitor = member.Monitor();
            var sut = member.GetState();

            // act
            sut.BePromoted();

            // assert
            sut.Guild.Should().NotBeNull().And.BeOfType<NullGuild>().And.Be(member.GetGuild());
            sut.IsGuildLeader.Should().BeFalse().And.Be(member.IsGuildLeader);
            sut.Guild.Members.Should().NotContain(member);
            member.GuildId.Should().BeNull();

            monitor.AssertCollectionNotChanged(member.Memberships);
            monitor.AssertPropertyNotChanged(
                nameof(Member.Id),
                nameof(Member.Name),
                nameof(Guild),
                nameof(Member.GuildId),
                nameof(Member.IsGuildLeader));
        }

        [Fact]
        public void BeDemoted_Should_Change_Nothing()
        {
            // arrange
            var member = (MemberTestProxy)MemberFake.WithoutGuild().Generate();
            var monitor = member.Monitor();
            var sut = member.GetState();

            // act
            sut.BeDemoted();

            // assert
            sut.Guild.Should().NotBeNull().And.BeOfType<NullGuild>().And.Be(member.GetGuild());
            sut.IsGuildLeader.Should().BeFalse().And.Be(member.IsGuildLeader);
            sut.Guild.Members.Should().NotContain(member);
            member.GuildId.Should().BeNull();

            monitor.AssertCollectionNotChanged(member.Memberships);
            monitor.AssertPropertyNotChanged(
                nameof(Member.Id),
                nameof(Member.Name),
                nameof(Guild),
                nameof(Member.GuildId),
                nameof(Member.IsGuildLeader));
        }

        [Fact]
        public void Leave_Should_Change_Nothing()
        {
            // arrange
            var member = (MemberTestProxy)MemberFake.WithoutGuild().Generate();
            var monitor = member.Monitor();
            var sut = member.GetState();

            // act
            sut.Leave();

            // assert
            sut.Guild.Should().BeOfType<NullGuild>().And.Be(member.GetGuild());
            sut.Guild.Members.Should().NotContain(member);
            sut.IsGuildLeader.Should().BeFalse().And.Be(member.IsGuildLeader);
            member.GuildId.Should().BeNull();

            monitor.AssertCollectionNotChanged(member.Memberships);
            monitor.AssertPropertyNotChanged(
                nameof(Guild),
                nameof(Member.Id),
                nameof(Member.Name),
                nameof(Member.IsGuildLeader),
                nameof(Member.GuildId));
        }
    }
}