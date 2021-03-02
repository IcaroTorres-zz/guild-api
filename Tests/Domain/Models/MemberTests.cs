using Bogus;
using Domain.Models;
using Domain.Nulls;
using FluentAssertions;
using Tests.Domain.Models.Fakes;
using Tests.Helpers;
using Xunit;

namespace Tests.Domain.Models
{
    [Trait("Domain", "Model")]
    public class MemberTests
    {
        [Fact]
        public void Constructor_WithName_Should_CreateWith_Name_Id()
        {
            // arrange
            var expectedName = new Person().UserName;

            // act
            var sut = new Member(expectedName);

            // assert
            sut.Should().NotBeNull().And.BeOfType<Member>();
            sut.Id.Should().NotBeEmpty();
            sut.Name.Should().NotBeEmpty().And.Be(expectedName);
            sut.IsGuildLeader.Should().BeFalse();
            sut.GuildId.Should().BeNull();
            sut.Guild.Should().BeOfType<NullGuild>();
            sut.Memberships.Should().BeEmpty();
        }

        [Fact]
        public void ChangeName_WithName_Should_Change_NameOnly()
        {
            // arrange
            var expectedName = new Person().UserName;
            var sut = MemberFake.WithoutGuild().Generate();
            var monitor = sut.Monitor();

            // act
            sut.ChangeName(expectedName);

            // assert
            sut.Should().NotBeNull().And.BeOfType<Member>();
            sut.Name.Should().NotBeEmpty().And.Be(expectedName);

            monitor.AssertPropertyChanged(nameof(Member.Name));
            monitor.AssertCollectionNotChanged(sut.Memberships);
            monitor.AssertPropertyNotChanged(
                nameof(Member.Id),
                nameof(Member.IsGuildLeader),
                nameof(Member.GuildId),
                nameof(Member.Guild));
        }
    }
}
