using Domain.Models.States.Memberships;
using FluentAssertions;
using System;
using Tests.Domain.Models.Fakes;
using Xunit;

namespace Tests.Domain.Models.States.Memberships
{
    [Trait("Domain", "Model-State")]
	public class OpenMembershipStateTests
	{
		[Fact]
		public void Constructor_Should_CreateWithout_ModifiedDate()
		{
			// arrange
			var membership = MembershipFake.Active().Generate();

			// act
			var sut = new OpenMembershipState(membership);

			// assert
			sut.ModifiedDate.Should().BeNull().And.Be(membership.ModifiedDate);
		}

		[Fact]
		public void Finish_Should_Change_ModifiedDate()
		{
			// arrange
			var referenceDate = DateTime.UtcNow;
			var membership = MembershipFake.Active().Generate();
			var sut = new OpenMembershipState(membership);

			// act
			sut.Finish();

			// assert
			sut.ModifiedDate.Should().BeNull().And.NotBe(membership.ModifiedDate.Value);
			membership.ModifiedDate.Should().NotBeNull().And.BeAfter(referenceDate);
		}
	}
}