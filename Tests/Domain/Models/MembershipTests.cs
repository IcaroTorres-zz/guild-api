using FluentAssertions;
using Tests.Domain.Models.Fakes;
using Tests.Domain.Models.TestModels;
using Xunit;

namespace Tests.Domain.Models
{
    [Trait("Domain", "Model")]
    public class MembershipTests
    {

        [Fact]
        public void GetDuration_FinishedMembership_Should_Return_FixedDuration()
        {
            // arrange
            var sut = MembershipFake.Finished().Generate();
            var expectedDuration = sut.ModifiedDate.Value.Subtract(sut.CreatedDate);

            // act
            var result = sut.GetDuration();

            // assert
            sut.Should().NotBeNull().And.BeOfType<TestMembership>();
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
            sut.Should().NotBeNull().And.BeOfType<TestMembership>();
            sut.ModifiedDate.Should().BeNull();
            result1.Should().BeLessThan(result2);
        }
    }
}
