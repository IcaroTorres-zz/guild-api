using Domain.States.Memberships;
using FluentAssertions;
using Tests.Domain.Models.Fakes;
using Xunit;

namespace Tests.Domain.States.Memberships
{
    [Trait("Domain", "Model-State")]
    public class ClosedMembershipStateTests
    {
        [Fact]
        public void Constructor_Should_CreateWith_ModifiedDate()
        {
            // arrange
            var membership = MembershipFake.Finished().Generate();

            // act
            var sut = new ClosedMembershipState(membership, membership.ModifiedDate.Value);

            // assert
            sut.ModifiedDate.Should().NotBeNull().And.Be(membership.ModifiedDate);
        }

        [Fact]
        public void Finish_Should_Change_Nothing()
        {
            // arrange
            var membership = MembershipFake.Finished().Generate();
            var sut = membership.GetState();

            // act
            sut.Finish();

            // assert
            sut.ModifiedDate.Should().NotBeNull().And.Be(membership.ModifiedDate);
        }
    }
}