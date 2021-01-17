using Domain.Models.Nulls;
using Domain.Models.States.Members;
using FluentAssertions;
using Tests.Domain.Models.Fakes;
using Xunit;

namespace Tests.Domain.Models.States.Members
{
    [Trait("Domain", "Model-State")]
	public class NoGuildMemberStateTests
    {
		[Fact]
		public void Constructor_Should_CreateWith_GivenStatus()
		{
			// arrange
			var member = MemberFake.WithoutGuild().Generate();

			// act
			var sut = new NoGuildMemberState(member);

			// assert
			sut.Guild.Should().BeOfType<NullGuild>().And.Be(member.Guild);
			sut.IsGuildLeader.Should().BeFalse().And.Be(member.IsGuildLeader);
			sut.Guild.Members.Should().NotContain(member);
		}

		[Fact]
		public void BePromoted_Should_Change_Nothing()
		{
			// arrange
			var member = MemberFake.WithoutGuild().Generate();
			var sut = new NoGuildMemberState(member);

			// act
			sut.BePromoted();

			// assert
			sut.Guild.Should().BeOfType<NullGuild>().And.Be(member.Guild);
			sut.IsGuildLeader.Should().BeFalse().And.Be(member.IsGuildLeader);
			sut.Guild.Members.Should().NotContain(member);
		}

		[Fact]
		public void BeDemoted_Should_Change_Nothing()
		{
			// arrange
			var member = MemberFake.WithoutGuild().Generate();
			var sut = new NoGuildMemberState(member);

			// act
			sut.BeDemoted();

			// assert
			sut.Guild.Should().BeOfType<NullGuild>().And.Be(member.Guild);
			sut.IsGuildLeader.Should().BeFalse().And.Be(member.IsGuildLeader);
			sut.Guild.Members.Should().NotContain(member);
		}

		[Fact]
		public void Leave_Should_Change_Nothing()
		{
			// arrange
			var member = MemberFake.WithoutGuild().Generate();
			var sut = new NoGuildMemberState(member);

			// act
			sut.Leave();

			// assert
			sut.Guild.Should().BeOfType<NullGuild>().And.Be(member.Guild);
			sut.IsGuildLeader.Should().BeFalse().And.Be(member.IsGuildLeader);
			sut.Guild.Members.Should().NotContain(member);
		}
	}
}