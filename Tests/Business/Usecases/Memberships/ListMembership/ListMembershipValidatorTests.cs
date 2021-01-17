using Business.Usecases.Memberships.ListMemberships;
using FluentAssertions;
using FluentValidation.Results;
using Tests.Helpers;
using Xunit;

namespace Tests.Business.Usecases.Memberships.ListMembership
{
    [Trait("Business", "Validator")]
    public class ListMembershipValidatorTests
    {
        [Fact]
        public void Should_Succeed()
        {
            // arrange
            var command = ListMembershipCommandFake.Valid().Generate();
            var sut = new ListMembershipValidator
            {
                CascadeMode = FluentValidation.CascadeMode.Stop
            };

            // act
            var result = sut.Validate(command);

            // assert
            result.Should().NotBeNull()
                .And.BeOfType<ValidationResult>();
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Should_Fail_By_Page_LessThan0()
        {
            // arrange
            var command = ListMembershipCommandFake.InvalidByPage().Generate();
            var sut = new ListMembershipValidator();

            // act
            var result = sut.Validate(command);

            // assert
            result.AssertErrorsCount(1);
        }

        [Fact]
        public void Should_Fail_By_PageSize_LessThan0()
        {
            // arrange
            var command = ListMembershipCommandFake.InvalidByPageSize().Generate();
            var sut = new ListMembershipValidator();

            // act
            var result = sut.Validate(command);

            // assert
            result.AssertErrorsCount(1);
        }
    }
}
