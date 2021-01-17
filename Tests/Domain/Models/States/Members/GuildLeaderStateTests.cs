using Domain.Models;
using Domain.Models.Nulls;
using Domain.Models.States.Members;
using FluentAssertions;
using Tests.Domain.Models.Fakes;
using Xunit;

namespace Tests.Domain.Models.States.Members
{
    [Trait("Domain", "Model-State")]
	public class GuildLeaderStateTests
    {
		[Fact]
		public void Constructor_Should_CreateWith_GivenStatus()
		{
			// arrange
			var leader = MemberFake.GuildLeader().Generate();

			// act
			var sut = new GuildLeaderState(leader, leader.Guild);

			// assert
			sut.Guild.Should().BeOfType<Guild>().And.Be(leader.Guild);
			sut.IsGuildLeader.Should().BeTrue().And.Be(leader.IsGuildLeader);
			sut.Guild.Members.Should().Contain(leader);
		}

		[Fact]
		public void BePromoted_Should_Change_IsGuildLeader()
		{
			// arrange
			var leader = MemberFake.GuildLeader().Generate();
			var sut = new GuildLeaderState(leader, leader.Guild);

			// act
			sut.BePromoted();

			// assert
			sut.Guild.Should().BeOfType<Guild>().And.Be(leader.Guild);
			sut.IsGuildLeader.Should().BeTrue().And.Be(leader.IsGuildLeader);
			sut.Guild.Members.Should().Contain(leader);
		}

		[Fact]
		public void BeDemoted_Should_Change_Nothing()
		{
			// arrange
			var leader = MemberFake.GuildLeader().Generate();
			var sut = new GuildLeaderState(leader, leader.Guild);

			// act
			sut.BeDemoted();

			// assert
			sut.Guild.Should().BeOfType<Guild>().And.Be(leader.Guild);
			sut.IsGuildLeader.Should().BeTrue().And.Be(!leader.IsGuildLeader);
			sut.Guild.Members.Should().Contain(leader);
		}

		[Fact]
		public void Leave_Should_Change_Guild()
		{
			// arrange
			var leader = MemberFake.GuildLeader().Generate();
			var sut = new GuildLeaderState(leader, leader.Guild);

			// act
			sut.Leave();

			// assert
			sut.Guild.Should().BeOfType<Guild>().And.NotBe(leader.Guild);
			sut.IsGuildLeader.Should().BeTrue().And.Be(!leader.IsGuildLeader);
			sut.Guild.Members.Should().NotContain(leader);
			leader.Guild.Should().BeOfType<NullGuild>();
		}
	}
}