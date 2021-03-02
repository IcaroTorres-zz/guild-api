using Domain.Models;
using FluentAssertions;
using Tests.Domain.Models.Fakes;
using Xunit;

namespace Tests.Domain.Models
{
    [Trait("Domain", "Model")]
    public class MembershipTests
    {
        [Fact]
        public void Constructor_WithGuildAndMember_Should_CreateWith_AllProperties_Except_ModifiedDate()
        {
            // arrange
            var guild = GuildFake.Valid().Generate();
            var member = MemberFake.WithoutGuild().Generate();

            // act
            var sut = new Membership(guild, member);

            // assert
            sut.Should().NotBeNull().And.BeOfType<Membership>();
            sut.Id.Should().NotBeEmpty();
            sut.ModifiedDate.Should().BeNull();
            sut.Guild.Should().Be(guild);
            sut.GuildId.Should().NotBeEmpty().And.Be(guild.Id);
            sut.Member.Should().Be(member);
            sut.MemberId.Should().NotBeEmpty().And.Be(member.Id);
        }

        [Fact]
        public void GetDuration_FinishedMembership_Should_Return_FixedDuration()
        {
            // arrange
            var sut = MembershipFake.Finished().Generate();
            var expectedDuration = sut.ModifiedDate.Value.Subtract(sut.CreatedDate);

            // act
            var result = sut.GetDuration();

            // assert
            sut.Should().NotBeNull().And.BeOfType<Membership>();
            sut.ModifiedDate.Should().NotBeNull();
            result.Should().Be(expectedDuration);
        }

        [Fact]
        public void GetDuration_ActiveMembership_Should_Return_VariantDuration()
        {
            // arrange
            var sut = MembershipFake.Active().Generate();

            // act
            var result1 = sut.GetDuration();
            var result2 = sut.GetDuration();

            // assert
            sut.Should().NotBeNull().And.BeOfType<Membership>();
            sut.ModifiedDate.Should().BeNull();
            result1.Should().BeLessThan(result2);
        }
    }
}
