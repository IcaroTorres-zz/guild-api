using Application.Invites.Queries.ListInvite;
using FluentAssertions;
using FluentValidation.Results;
using Tests.Helpers;
using Xunit;

namespace Tests.Application.Invites.Queries.ListInvite
{
    [Trait("Application", "Validator")]
    public class ListInviteValidatorTests
    {
        [Fact]
        public void Should_Succeed()
        {
            // arrange
            var command = ListInviteCommandFake.Valid().Generate();
            var sut = new ListInviteValidator
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
            var command = ListInviteCommandFake.InvalidByPage().Generate();
            var sut = new ListInviteValidator
            {
                CascadeMode = FluentValidation.CascadeMode.Stop
            };

            // act
            var result = sut.Validate(command);

            // assert
            result.AssertErrorsCount(1);
        }

        [Fact]
        public void Should_Fail_By_PageSize_LessThan0()
        {
            // arrange
            var command = ListInviteCommandFake.InvalidByPageSize().Generate();
            var sut = new ListInviteValidator
            {
                CascadeMode = FluentValidation.CascadeMode.Stop
            };

            // act
            var result = sut.Validate(command);

            // assert
            result.AssertErrorsCount(1);
        }
    }
}
