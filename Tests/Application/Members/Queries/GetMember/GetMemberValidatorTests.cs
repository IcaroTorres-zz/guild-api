using Application.Members.Queries.GetMember;
using FluentAssertions;
using FluentValidation.Results;
using Tests.Helpers;
using Xunit;

namespace Tests.Application.Members.Queries.GetMember
{
    [Trait("Application", "Validator")]
    public class GetMemberValidatorTests
    {
        [Fact]
        public void Should_Succeed()
        {
            // arrange
            var command = GetMemberCommandFake.Valid().Generate();
            var sut = new GetMemberValidator
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
            var command = GetMemberCommandFake.InvalidByEmptyId().Generate();
            var sut = new GetMemberValidator
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
