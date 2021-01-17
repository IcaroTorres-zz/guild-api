using Business.Usecases.Invites.GetInvite;
using FluentAssertions;
using FluentValidation.Results;
using Tests.Helpers;
using Xunit;

namespace Tests.Business.Usecases.Invites.GetInvite
{
    [Trait("Business", "Validator")]
    public class GetInviteValidatorTests
    {
        [Fact]
        public void Should_Succeed()
        {
            // arrange
            var command = GetInviteCommandFake.Valid().Generate();
            var sut = new GetInviteValidator
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
        public void Should_Fail_By_Empty_Id()
        {
            // arrange
            var command = GetInviteCommandFake.InvalidByEmptyId().Generate();
            var sut = new GetInviteValidator
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
