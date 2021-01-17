using Domain.Models;
using Domain.Models.Nulls;
using Domain.Models.States.Members;
using FluentAssertions;
using Tests.Domain.Models.Fakes;
using Xunit;

namespace Tests.Domain.Models.States.Members
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
		public void BePromoted_Should_Change_IsGuildLeader()
		{
			// arrange
			var member = MemberFake.GuildMember().Generate();
			var sut = new GuildMemberState(member, member.Guild);

			// act
			sut.BePromoted();

			// assert
			sut.Guild.Should().BeOfType<Guild>().And.Be(member.Guild);
			sut.IsGuildLeader.Should().BeFalse().And.Be(!member.IsGuildLeader);
			sut.Guild.Members.Should().Contain(member);
		}

		[Fact]
		public void BeDemoted_Should_Change_Nothing()
		{
			// arrange
			var member = MemberFake.GuildMember().Generate();
			var sut = new GuildMemberState(member, member.Guild);

			// act
			sut.BeDemoted();

			// assert
			sut.Guild.Should().BeOfType<Guild>().And.Be(member.Guild);
			sut.IsGuildLeader.Should().BeFalse().And.Be(member.IsGuildLeader);
			sut.Guild.Members.Should().Contain(member);
		}

		[Fact]
		public void Leave_Should_Change_Guild()
		{
			// arrange
			var member = MemberFake.GuildMember().Generate();
			var sut = new GuildMemberState(member, member.Guild);

			// act
			sut.Leave();

			// assert
			sut.Guild.Should().BeOfType<Guild>().And.NotBe(member.Guild);
			sut.IsGuildLeader.Should().BeFalse().And.Be(member.IsGuildLeader);
			sut.Guild.Members.Should().NotContain(member);
			member.Guild.Should().BeOfType<NullGuild>();
		}
	}
}