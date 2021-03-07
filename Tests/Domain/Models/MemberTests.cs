using Bogus;
using Domain.Models;
using FluentAssertions;
using Tests.Domain.Models.Fakes;
using Tests.Domain.Models.TestModels;
using Tests.Helpers;
using Xunit;

namespace Tests.Domain.Models
{
    [Trait("Domain", "Model")]
    public class MemberTests
    {
        [Fact]
        public void ChangeName_WithName_Should_Change_NameOnly()
        {
            // arrange
            var expectedName = new Person().UserName;
            var sut = (TestMember)MemberFake.WithoutGuild().Generate();
            var monitor = sut.Monitor();

            // act
            sut.ChangeName(expectedName);

            // assert
            sut.Should().NotBeNull().And.BeOfType<TestMember>();
            sut.Name.Should().NotBeEmpty().And.Be(expectedName);

            monitor.AssertPropertyChanged(nameof(Member.Name));
            monitor.AssertCollectionNotChanged(sut.Memberships);
            monitor.AssertPropertyNotChanged(
                nameof(Member.Id),
                nameof(Member.IsGuildLeader),
                nameof(Member.GuildId),
                nameof(Guild));
        }
    }
}
